using PasswordManager.Models.Base;
using PasswordManager.Models.UserData;
using PasswordManager.Services;
using PasswordManager.Services.Utils;
using PasswordManager.UI;

namespace PasswordManager.Core
{
    public class AppRunner(DependencyContainer container)
    {
        private readonly IAuthService            _authService      = container.AuthService;
        private readonly IVaultService           _vaultService     = container.VaultService;
        private readonly IPasswordGeneratorService _passwordGenerator = container.PasswordGenerator;
        private readonly IMenuHandler            _menu             = container.Menu;
        private readonly IDisplayHandler         _display          = container.Display;

        public void Run()
        {
            try
            {
                bool keepRunning = true;
                while (keepRunning)
                    keepRunning = HandleAuthFlow();
            }
            finally
            {
                _authService.Logout();
            }
        }

        private bool HandleAuthFlow()
        {
            var option = _menu.PromptAuthMenu();

            if (option == AuthMenuOption.Exit)
                return false;

            string password = _menu.PromptMasterPassword("Enter Master Password: ");
            ResponseMsg result = option == AuthMenuOption.Register
                ? _authService.Register(password)
                : _authService.Login(password);

            _display.ShowResult(result);

            if (result.IsSuccess)
            {
                bool continueApp = RunVaultLoop();
                if (!continueApp)
                    return false;
            }

            return true;
        }

        private bool RunVaultLoop()
        {
            while (true)
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
        }

        private void RunCategoryLoop()
        {
            while (true)
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
                        return;
                }
            }
        }

        private void HandleListEntries()
        {
            var result = _vaultService.GetAllEntries();
            if (result.IsSuccess && result.Data != null)
                _display.ShowEntries(result.Data);
            else
                _display.ShowResult(result);
        }

        private void HandleAddEntry()
        {
            var categories = GetAvailableCategories();
            var entry      = _menu.PromptNewEntry(categories);
            var result     = _vaultService.AddEntry(entry);
            _display.ShowResult(result);
        }

        private void HandleUpdateEntry()
        {
            int id          = _menu.PromptId("Enter Entry ID to update: ");
            var entryResult = _vaultService.GetEntryById(id);

            if (!entryResult.IsSuccess || entryResult.Data == null)
            {
                _display.ShowResult(entryResult);
                return;
            }

            var categories = GetAvailableCategories();
            var updateDto  = _menu.PromptUpdateEntry(categories);
            var result     = _vaultService.UpdateEntry(id, updateDto);
            _display.ShowResult(result);
        }

        private void HandleDeleteEntry()
        {
            int id     = _menu.PromptId("Enter Entry ID to delete: ");
            var result = _vaultService.DeleteEntry(id);
            _display.ShowResult(result);
        }

        private void HandleSearchEntries()
        {
            string query   = _menu.PromptKeyword();
            var result     = _vaultService.GetAllEntries();

            if (!result.IsSuccess || result.Data == null)
            {
                _display.ShowResult(result);
                return;
            }

            var filtered = result.Data
                .Where(e =>
                    e.Website.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    e.Email.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            _display.ShowEntries(filtered);
        }

        private void HandleRevealPassword()
        {
            int id     = _menu.PromptId("Enter Entry ID to reveal password: ");
            var result = _vaultService.GetEntryById(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _display.ShowResult(result);
                return;
            }

            // Password in DB is encrypted — decrypt before showing
            var decrypted = _vaultService.RevealPassword(result.Data);
            _display.ShowPassword(decrypted);
        }

        private void HandleGeneratePassword()
        {
            var options = _menu.PromptPasswordOptions();

            var generateResult = _passwordGenerator.TryGenerate(options, out string? password, out string? error);

            if (!generateResult)
            {
                _display.ShowResult(ResponseMsg.Failure(error ?? "Failed to generate password."));
                return;
            }

            var strength = _passwordGenerator.CheckStrength(password!);
            _display.ShowPassword(password!);
            _display.ShowPasswordStrength(strength);
        }

        private void HandleChangePassword()
        {
            _display.ShowResult(ResponseMsg.Failure("Change Password is not yet implemented."));
        }

        private void HandleListCategories()
        {
            var result = _vaultService.GetAllCategories();
            if (result.IsSuccess && result.Data != null)
                _display.ShowCategories(result.Data);
            else
                _display.ShowResult(result);
        }

        private void HandleAddCategory()
        {
            var category = _menu.PromptNewCategory();
            var result   = _vaultService.AddCategory(category);
            _display.ShowResult(result);
        }

        private void HandleDeleteCategory()
        {
            int id     = _menu.PromptId("Enter Category ID to delete: ");
            var result = _vaultService.DeleteCategory(id);
            _display.ShowResult(result);
        }

        private List<CategoryData> GetAvailableCategories()
        {
            var result = _vaultService.GetAllCategories();
            return result.IsSuccess && result.Data != null
                ? result.Data
                : [];
        }
    }
}
