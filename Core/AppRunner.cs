using PasswordManager.Models.Base;
using PasswordManager.Models.UserData;
using PasswordManager.Services;
using PasswordManager.Services.Utils;
using PasswordManager.UI;

namespace PasswordManager.Core
{
    public class AppRunner(DependencyContainer container)
    {
        private readonly IAuthService _authService = container.AuthService;
        private readonly IVaultService _vaultService = container.VaultService;
        private readonly IPasswordGeneratorService _passwordGenerator = container.PasswordGenerator;
        private readonly ConsoleMenu _menu = container.Menu;
        private readonly ConsoleDisplayHelper _display = container.Display;

        public void Run()
        {
            try
            {
                bool keepRunning = true;
                while (keepRunning)
                {
                    keepRunning = HandleAuthFlow();
                }
            }
            finally
            {
                // Ensure Logout is always executed prior to application exit
                _authService.Logout();
            }
        }

        private bool HandleAuthFlow()
        {
            var option = _menu.PromptAuthMenu();
            if (option == AuthMenuOption.Exit)
            {
                return false;
            }

            string password = _menu.PromptMasterPassword("Enter Master Password: ");
            ResponseMsg result;

            if (option == AuthMenuOption.Register)
            {
                result = _authService.Register(password);
            }
            else
            {
                result = _authService.Login(password);
            }

            _display.ShowResult(result);

            if (result.IsSuccess)
            {
                bool continueApp = RunVaultLoop();
                if (!continueApp)
                {
                    return false;
                }
            }

            return true;
        }

        private bool RunVaultLoop()
        {
            bool inVault = true;
            while (inVault)
            {
                var option = _menu.PromptMainMenu();
                switch (option)
                {
                    case MainMenuOption.ListEntries:
                        HandleListEntries();
                        break;
                    case MainMenuOption.AddEntry:
                        HandleAddEntry();
                        break;
                    case MainMenuOption.UpdateEntry:
                        HandleUpdateEntry();
                        break;
                    case MainMenuOption.DeleteEntry:
                        HandleDeleteEntry();
                        break;
                    case MainMenuOption.SearchEntries:
                        HandleSearchEntries();
                        break;
                    case MainMenuOption.RevealPassword:
                        HandleRevealPassword();
                        break;
                    case MainMenuOption.GeneratePassword:
                        HandleGeneratePassword();
                        break;
                    case MainMenuOption.ChangePassword:
                        HandleChangePassword();
                        break;
                    case MainMenuOption.ManageCategories:
                        RunCategoryLoop();
                        break;
                    case MainMenuOption.Exit:
                        return false;
                }
            }
            return true;
        }

        private void RunCategoryLoop()
        {
            bool inCategoryMenu = true;
            while (inCategoryMenu)
            {
                var option = _menu.PromptCategoryMenu();
                switch (option)
                {
                    case CategoryMenuOption.ListCategories:
                        HandleListCategories();
                        break;
                    case CategoryMenuOption.AddCategory:
                        HandleAddCategory();
                        break;
                    case CategoryMenuOption.DeleteCategory:
                        HandleDeleteCategory();
                        break;
                    case CategoryMenuOption.Back:
                        inCategoryMenu = false;
                        break;
                }
            }
        }

        private void HandleListEntries()
        {
            var result = _vaultService.GetAllEntries();
            _display.ShowResult(result);
            if (result.IsSuccess && result.Data != null)
            {
                _display.ShowEntries(result.Data);
            }
        }

        private void HandleAddEntry()
        {
            List<CategoryData> categories = GetAvailableCategories();
            var entry = _menu.PromptNewEntry(categories);
            var result = _vaultService.AddEntry(entry);
            _display.ShowResult(result);
        }

        private void HandleUpdateEntry()
        {
            int id = _menu.PromptId("Enter Entry ID to update: ");
            var entryResult = _vaultService.GetEntryById(id);
            _display.ShowResult(entryResult);
            
            if (!entryResult.IsSuccess || entryResult.Data == null)
            {
                return;
            }

            List<CategoryData> categories = GetAvailableCategories();
            var updateDto = _menu.PromptUpdateEntry(categories);
            var result = _vaultService.UpdateEntry(id, updateDto);
            _display.ShowResult(result);
        }

        private void HandleDeleteEntry()
        {
            int id = _menu.PromptId("Enter Entry ID to delete: ");
            var result = _vaultService.DeleteEntry(id);
            _display.ShowResult(result);
        }

        private void HandleSearchEntries()
        {
            // IVaultService.SearchEntries() is not implemented yet in the database,
            // so we handle this gracefully via a safe client-side filter.
            Console.Write("Enter search term (website or email): ");
            string query = Console.ReadLine()?.Trim() ?? string.Empty;

            var result = _vaultService.GetAllEntries();
            if (result.IsSuccess && result.Data != null)
            {
                var filtered = result.Data
                    .Where(e => e.Website.Contains(query, StringComparison.OrdinalIgnoreCase) || 
                                e.Email.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                _display.ShowEntries(filtered);
            }
            else
            {
                _display.ShowResult(result);
            }
        }

        private void HandleRevealPassword()
        {
            int id = _menu.PromptId("Enter Entry ID to reveal password: ");
            var result = _vaultService.GetEntryById(id);
            _display.ShowResult(result);
            if (result.IsSuccess && result.Data != null)
            {
                _display.ShowPassword(result.Data.Password);
            }
        }

        private void HandleGeneratePassword()
        {
            var options = _menu.PromptPasswordOptions();
            try
            {
                string password = _passwordGenerator.Generate(options);
                var strength = _passwordGenerator.CheckStrength(password);
                _display.ShowPassword(password);
                _display.ShowPasswordStrength(strength);
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERROR] {ex.Message}");
                Console.ResetColor();
            }
        }

        private void HandleChangePassword()
        {
            // IAuthService.ChangePassword() is currently not implemented on the Auth service.
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Change Password is not implemented in the current Service.");
            Console.ResetColor();
        }

        private void HandleListCategories()
        {
            var result = _vaultService.GetAllCategory();
            if (result.IsSuccess && result.Data != null)
            {
                var categories = ExtractCategoryList(result.Data);
                _display.ShowCategories(categories);
            }
            else
            {
                _display.ShowResult(result);
            }
        }

        private void HandleAddCategory()
        {
            var category = _menu.PromptNewCategory();
            var result = _vaultService.AddCategory(category);
            _display.ShowResult(result);
        }

        private void HandleDeleteCategory()
        {
            int id = _menu.PromptId("Enter Category ID to delete: ");
            var result = _vaultService.DeleteCategory(id);
            _display.ShowResult(result);
        }

        private List<CategoryData> GetAvailableCategories()
        {
            var catResponse = _vaultService.GetAllCategory();
            if (catResponse.IsSuccess && catResponse.Data != null)
            {
                return ExtractCategoryList(catResponse.Data);
            }
            return new List<CategoryData>();
        }

        /// <summary>
        /// Converts the service results securely. Fits whichever way the database was built:
        /// extracts Category navigation property if List of CoreDataModel is returned, or
        /// casts directly to List of CategoryData if the interface has an implicit design cast.
        /// </summary>
        private List<CategoryData> ExtractCategoryList(object data)
        {
            if (data is List<CategoryData> directList)
            {
                return directList;
            }

            if (data is List<CoreDataModel> modelList)
            {
                return modelList
                    .Select(e => e.Category)
                    .Where(c => c != null)
                    .GroupBy(c => c!.CategoryDataId)
                    .Select(g => g.First()!)
                    .ToList();
            }

            return new List<CategoryData>();
        }
    }
}
