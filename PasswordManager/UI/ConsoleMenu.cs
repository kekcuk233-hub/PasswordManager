using PasswordManager.Models.DTO;
using PasswordManager.Models.UserData;
using PasswordManager.Services.Utils;

namespace PasswordManager.UI
{
    public class ConsoleMenu : IMenuHandler
    {
        public AuthMenuOption PromptAuthMenu()
        {
            Console.WriteLine("\n=== Password Manager ===");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("0. Exit");
            Console.Write("\nChoice: ");

            return Console.ReadLine()?.Trim() switch
            {
                "1" => AuthMenuOption.Login,
                "2" => AuthMenuOption.Register,
                "0" => AuthMenuOption.Exit,
                _   => PromptAuthMenu()
            };
        }

        public MainMenuOption PromptMainMenu()
        {
            Console.WriteLine("\n=== Vault ===");
            Console.WriteLine("1. List entries");
            Console.WriteLine("2. Add entry");
            Console.WriteLine("3. Update entry");
            Console.WriteLine("4. Delete entry");
            Console.WriteLine("5. Search entries");
            Console.WriteLine("6. Reveal password");
            Console.WriteLine("7. Generate password");
            Console.WriteLine("8. Change master password");
            Console.WriteLine("9. Manage categories");
            Console.WriteLine("0. Exit");
            Console.Write("\nChoice: ");

            return Console.ReadLine()?.Trim() switch
            {
                "1" => MainMenuOption.ListEntries,
                "2" => MainMenuOption.AddEntry,
                "3" => MainMenuOption.UpdateEntry,
                "4" => MainMenuOption.DeleteEntry,
                "5" => MainMenuOption.SearchEntries,
                "6" => MainMenuOption.RevealPassword,
                "7" => MainMenuOption.GeneratePassword,
                "8" => MainMenuOption.ChangePassword,
                "9" => MainMenuOption.ManageCategories,
                "0" => MainMenuOption.Exit,
                _   => PromptMainMenu()
            };
        }

        public CategoryMenuOption PromptCategoryMenu()
        {
            Console.WriteLine("\n=== Categories ===");
            Console.WriteLine("1. List categories");
            Console.WriteLine("2. Add category");
            Console.WriteLine("3. Delete category");
            Console.WriteLine("0. Back");
            Console.Write("\nChoice: ");

            return Console.ReadLine()?.Trim() switch
            {
                "1" => CategoryMenuOption.ListCategories,
                "2" => CategoryMenuOption.AddCategory,
                "3" => CategoryMenuOption.DeleteCategory,
                "0" => CategoryMenuOption.Back,
                _   => PromptCategoryMenu()
            };
        }

        public string PromptMasterPassword(string prompt)
        {
            Console.Write(prompt);
            var password = string.Empty;

            ConsoleKeyInfo key;
            while ((key = Console.ReadKey(intercept: true)).Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[..^1];
                    Console.Write("\b \b");
                }
                else if (key.Key != ConsoleKey.Backspace)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            }

            Console.WriteLine();
            return password;
        }

        public int PromptId(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int id) && id > 0)
                    return id;
                Console.WriteLine("Invalid ID. Please enter a positive number.");
            }
        }

        public string PromptKeyword()
        {
            Console.Write("Search keyword: ");
            return Console.ReadLine() ?? string.Empty;
        }

        public CoreDataModel PromptNewEntry(List<CategoryData> categories)
        {
            Console.Write("Website: ");
            var website = Console.ReadLine() ?? string.Empty;

            Console.Write("Email: ");
            var email = Console.ReadLine() ?? string.Empty;

            Console.Write("Password: ");
            var password = Console.ReadLine() ?? string.Empty;

            Console.Write("URL (optional): ");
            var url = Console.ReadLine();

            Console.Write("Description (optional): ");
            var description = Console.ReadLine();

            int? categoryId = PromptCategorySelection(categories);

            return new CoreDataModel
            {
                Website          = website,
                Email            = email,
                Password         = password,
                Url              = string.IsNullOrWhiteSpace(url)         ? null : url,
                Description      = string.IsNullOrWhiteSpace(description) ? null : description,
                CategoryId       = categoryId,
                CreationDate     = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow
            };
        }

        public UpdateDto PromptUpdateEntry(List<CategoryData> categories)
        {
            Console.Write("Website: ");
            var website = Console.ReadLine();

            Console.Write("Email: ");
            var email = Console.ReadLine();

            Console.Write("Password: ");
            var password = Console.ReadLine();

            Console.Write("URL (optional): ");
            var url = Console.ReadLine();

            Console.Write("Description (optional): ");
            var description = Console.ReadLine();

            int? categoryId = PromptCategorySelection(categories);

            return new UpdateDto
            {
                Website          = string.IsNullOrWhiteSpace(website)     ? null : website,
                Email            = string.IsNullOrWhiteSpace(email)       ? null : email,
                Password         = string.IsNullOrWhiteSpace(password)    ? null : password,
                Url              = string.IsNullOrWhiteSpace(url)         ? null : url,
                Description      = string.IsNullOrWhiteSpace(description) ? null : description,
                CategoryId       = categoryId,
                LastModifiedDate = DateTime.UtcNow
            };
        }

        public CategoryData PromptNewCategory()
        {
            Console.Write("Category name: ");
            var name = Console.ReadLine() ?? string.Empty;

            Console.Write("Icon (optional, e.g. 📁): ");
            var icon = Console.ReadLine();

            return new CategoryData
            {
                CategoryName = name,
                Icon         = string.IsNullOrWhiteSpace(icon) ? null : icon
            };
        }

        public PasswordGeneratorService.PasswordOptions PromptPasswordOptions()
        {
            Console.Write("Length (default 16): ");
            var lengthInput = Console.ReadLine();
            int length = int.TryParse(lengthInput, out var parsed) ? parsed : 16;

            Console.Write("Include uppercase? (y/n, default y): ");
            bool upper = (Console.ReadLine()?.Trim().ToLower() ?? "y") != "n";

            Console.Write("Include digits? (y/n, default y): ");
            bool digits = (Console.ReadLine()?.Trim().ToLower() ?? "y") != "n";

            Console.Write("Include symbols? (y/n, default y): ");
            bool symbols = (Console.ReadLine()?.Trim().ToLower() ?? "y") != "n";

            return new PasswordGeneratorService.PasswordOptions
            {
                Length       = length,
                UseUppercase = upper,
                UseDigits    = digits,
                UseSymbols   = symbols
            };
        }

        private int? PromptCategorySelection(List<CategoryData> categories)
        {
            if (categories.Count == 0)
                return null;

            Console.WriteLine("Available categories:");
            foreach (var cat in categories)
                Console.WriteLine($"  {cat.CategoryDataId}. {cat.CategoryName} {cat.Icon}");

            Console.Write("Category ID (leave blank for default): ");
            var input = Console.ReadLine();

            return int.TryParse(input, out int id) ? id : null;
        }
    }
}
