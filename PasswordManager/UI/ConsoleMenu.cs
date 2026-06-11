using System;
using System.Collections.Generic;
using System.Text;
using PasswordManager.Models.Base;
using PasswordManager.Models.DTO;
using PasswordManager.Models.UserData;
using PasswordManager.Services;
using PasswordManager.Services.Utils;

namespace PasswordManager.UI
{
    public class ConsoleMenu
    {
        public AuthMenuOption PromptAuthMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== Authentication Menu ===");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");
                
                string? input = Console.ReadLine()?.Trim();
                if (input == "1") return AuthMenuOption.Login;
                if (input == "2") return AuthMenuOption.Register;
                if (input == "3") return AuthMenuOption.Exit;
                
                Console.WriteLine("Invalid option. Please enter 1, 2, or 3.");
            }
        }

        public MainMenuOption PromptMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== Main Menu ===");
                Console.WriteLine("1. List Entries");
                Console.WriteLine("2. Add Entry");
                Console.WriteLine("3. Update Entry");
                Console.WriteLine("4. Delete Entry");
                Console.WriteLine("5. Search Entries");
                Console.WriteLine("6. Reveal Password");
                Console.WriteLine("7. Generate Password");
                Console.WriteLine("8. Change Password");
                Console.WriteLine("9. Manage Categories");
                Console.WriteLine("10. Exit");
                Console.Write("Select an option: ");
                
                string? input = Console.ReadLine()?.Trim();
                switch (input)
                {
                    case "1": return MainMenuOption.ListEntries;
                    case "2": return MainMenuOption.AddEntry;
                    case "3": return MainMenuOption.UpdateEntry;
                    case "4": return MainMenuOption.DeleteEntry;
                    case "5": return MainMenuOption.SearchEntries;
                    case "6": return MainMenuOption.RevealPassword;
                    case "7": return MainMenuOption.GeneratePassword;
                    case "8": return MainMenuOption.ChangePassword;
                    case "9": return MainMenuOption.ManageCategories;
                    case "10": return MainMenuOption.Exit;
                    default:
                        Console.WriteLine("Invalid option. Please enter a number between 1 and 10.");
                        break;
                }
            }
        }

        public CategoryMenuOption PromptCategoryMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== Category Management ===");
                Console.WriteLine("1. List Categories");
                Console.WriteLine("2. Add Category");
                Console.WriteLine("3. Delete Category");
                Console.WriteLine("4. Back");
                Console.Write("Select an option: ");
                
                string? input = Console.ReadLine()?.Trim();
                if (input == "1") return CategoryMenuOption.ListCategories;
                if (input == "2") return CategoryMenuOption.AddCategory;
                if (input == "3") return CategoryMenuOption.DeleteCategory;
                if (input == "4") return CategoryMenuOption.Back;
                
                Console.WriteLine("Invalid option. Please enter 1, 2, 3, or 4.");
            }
        }

        public string PromptMasterPassword(string promptText)
        {
            Console.Write(promptText);
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    sb.Append(key.KeyChar);
                    Console.Write("*");
                }
            }
            return sb.ToString();
        }

        public int PromptId(string promptText)
        {
            while (true)
            {
                Console.Write(promptText);
                string? input = Console.ReadLine();
                if (int.TryParse(input, out int id) && id > 0)
                {
                    return id;
                }
                Console.WriteLine("Invalid ID. Please enter a valid positive integer.");
            }
        }

        public CoreDataModel PromptNewEntry(List<CategoryData>? categories)
        {
            Console.Write("Enter Website (required): ");
            string website = Console.ReadLine()?.Trim() ?? string.Empty;

            Console.Write("Enter Email/Username (required): ");
            string email = Console.ReadLine()?.Trim() ?? string.Empty;

            Console.Write("Enter Password (required): ");
            string password = Console.ReadLine()?.Trim() ?? string.Empty;

            Console.Write("Enter URL (optional, press Enter to skip): ");
            string? url = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(url)) url = null;

            Console.Write("Enter Description (optional, press Enter to skip): ");
            string? description = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(description)) description = null;

            int? categoryId = null;
            if (categories != null && categories.Count > 0)
            {
                Console.WriteLine("\nAvailable Categories:");
                foreach (var cat in categories)
                {
                    Console.WriteLine($"  - ID: {cat.CategoryDataId} | {cat.Icon ?? "📁"} {cat.CategoryName}");
                }
                Console.Write("Enter Category ID (or press Enter to skip): ");
                string? catInput = Console.ReadLine()?.Trim();
                if (int.TryParse(catInput, out int selectedCatId))
                {
                    categoryId = selectedCatId;
                }
            }

            return new CoreDataModel
            {
                Website = website,
                Email = email,
                Password = password,
                Url = url,
                Description = description,
                CategoryId = categoryId,
                CreationDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow
            };
        }

        public UpdateDto PromptUpdateEntry(List<CategoryData>? categories)
        {
            Console.Write("Enter new Website (press Enter to keep current): ");
            string? website = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(website)) website = null;

            Console.Write("Enter new Email/Username (press Enter to keep current): ");
            string? email = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(email)) email = null;

            Console.Write("Enter new Password (press Enter to keep current): ");
            string? password = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(password)) password = null;

            Console.Write("Enter new URL (press Enter to keep current): ");
            string? url = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(url)) url = null;

            Console.Write("Enter new Description (press Enter to keep current): ");
            string? description = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(description)) description = null;

            int? categoryId = null;
            bool changeCategory = false;
            if (categories != null && categories.Count > 0)
            {
                Console.WriteLine("\nAvailable Categories:");
                foreach (var cat in categories)
                {
                    Console.WriteLine($"  - ID: {cat.CategoryDataId} | {cat.Icon ?? "📁"} {cat.CategoryName}");
                }
                Console.Write("Enter new Category ID (or press Enter to keep current, or 'none' to clear): ");
                string? catInput = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(catInput))
                {
                    if (catInput.ToLower() == "none")
                    {
                        categoryId = null;
                        changeCategory = true;
                    }
                    else if (int.TryParse(catInput, out int selectedCatId))
                    {
                        categoryId = selectedCatId;
                        changeCategory = true;
                    }
                }
            }

            var dto = new UpdateDto
            {
                Website = website,
                Email = email,
                Password = password,
                Url = url,
                Description = description,
                LastModifiedDate = DateTime.UtcNow
            };

            if (changeCategory)
            {
                dto.CategoryId = categoryId;
            }

            return dto;
        }

        public PasswordGeneratorService.PasswordOptions PromptPasswordOptions()
        {
            Console.Write("Enter password length (default 16, min 4): ");
            string? lenInput = Console.ReadLine()?.Trim();
            int length = 16;
            if (int.TryParse(lenInput, out int len) && len >= 4)
            {
                length = len;
            }

            Console.Write("Use lowercase letters? (Y/n): ");
            bool useLowercase = Console.ReadLine()?.Trim().ToLower() != "n";

            Console.Write("Use uppercase letters? (Y/n): ");
            bool useUppercase = Console.ReadLine()?.Trim().ToLower() != "n";

            Console.Write("Use digits? (Y/n): ");
            bool useDigits = Console.ReadLine()?.Trim().ToLower() != "n";

            Console.Write("Use symbols? (Y/n): ");
            bool useSymbols = Console.ReadLine()?.Trim().ToLower() != "n";

            Console.Write("Exclude ambiguous characters (l, 1, I, O, 0)? (Y/n): ");
            bool noAmbiguous = Console.ReadLine()?.Trim().ToLower() != "n";

            return new PasswordGeneratorService.PasswordOptions
            {
                Length = length,
                UseLowercase = useLowercase,
                UseUppercase = useUppercase,
                UseDigits = useDigits,
                UseSymbols = useSymbols,
            };
        }

        public CategoryData PromptNewCategory()
        {
            Console.Write("Enter Category Name (required): ");
            string? name = Console.ReadLine()?.Trim();

            Console.Write("Enter Icon/Emoji (optional, press Enter to skip): ");
            string? icon = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(icon)) icon = null;

            return new CategoryData
            {
                CategoryName = name,
                Icon = icon,
                Entries = new List<CoreDataModel>()
            };
        }
    }
}
