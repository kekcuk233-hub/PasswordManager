using System;
using System.Collections.Generic;
using PasswordManager.Models.Base;
using PasswordManager.Models.UserData;
using PasswordManager.Services.Utils;

namespace PasswordManager.UI
{
    public class ConsoleDisplayHelper
    {
        public void ShowResult(ResponseMsg result)
        {
            if (result.IsSuccess)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[SUCCESS] {result.Message ?? "Operation completed successfully."}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERROR] {result.Message ?? "Operation failed."}");
            }
            Console.ResetColor();
        }

        public void ShowResult<T>(ResponseMsg<T> result) where T : class
        {
            if (result.IsSuccess)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[SUCCESS] {result.Message ?? "Operation completed successfully."}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERROR] {result.Message ?? "Operation failed."}");
            }
            Console.ResetColor();
        }

        public void ShowEntries(List<CoreDataModel> entries)
        {
            if (entries == null || entries.Count == 0)
            {
                Console.WriteLine("No entries to display.");
                return;
            }

            Console.WriteLine($"\nShowing {entries.Count} entries:");
            Console.WriteLine(new string('=', 60));

            foreach (var entry in entries)
            {
                string catName = entry.Category?.CategoryName ?? "None";
                if (!string.IsNullOrEmpty(entry.Category?.Icon))
                {
                    catName = $"{entry.Category.Icon} {catName}";
                }
                string catIdText = entry.CategoryId.HasValue ? $" (ID: {entry.CategoryId})" : "";

                Console.WriteLine($"{"ID:",-20} {entry.PasswordId}");
                Console.WriteLine($"{"Website:",-20} {entry.Website}");
                Console.WriteLine($"{"Email/Username:",-20} {entry.Email}");
                Console.WriteLine($"{"Password:",-20} ********");
                Console.WriteLine($"{"URL:",-20} {entry.Url ?? "N/A"}");
                Console.WriteLine($"{"Description:",-20} {entry.Description ?? "N/A"}");
                Console.WriteLine($"{"Category:",-20} {catName}{catIdText}");
                Console.WriteLine($"{"Created (UTC):",-20} {entry.CreationDate:yyyy-MM-dd HH:mm:ss}");
                Console.WriteLine($"{"Modified (UTC):",-20} {entry.LastModifiedDate:yyyy-MM-dd HH:mm:ss}");
                Console.WriteLine(new string('-', 60));
            }
            Console.WriteLine(new string('=', 60));
        }

        public void ShowEntry(CoreDataModel entry)
        {
            if (entry == null) return;
            Console.WriteLine("\n--- Entry Details ---");
            Console.WriteLine($"ID:            {entry.PasswordId}");
            Console.WriteLine($"Website:       {entry.Website}");
            Console.WriteLine($"Email:         {entry.Email}");
            Console.WriteLine($"Password:      ******** (Use Reveal to see)");
            Console.WriteLine($"URL:           {entry.Url ?? "N/A"}");
            Console.WriteLine($"Description:   {entry.Description ?? "N/A"}");
            
            string catName = entry.Category?.CategoryName ?? "None";
            if (!string.IsNullOrEmpty(entry.Category?.Icon))
            {
                catName = $"{entry.Category.Icon} {catName}";
            }
            string catIdText = entry.CategoryId.HasValue ? $" (ID: {entry.CategoryId})" : "";
            Console.WriteLine($"Category:      {catName}{catIdText}");
            Console.WriteLine($"Created:       {entry.CreationDate.ToLocalTime()}");
            Console.WriteLine($"Modified:      {entry.LastModifiedDate.ToLocalTime()}");
            Console.WriteLine(new string('-', 25));
        }

        public void ShowPassword(string password)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Revealed Password: {password}");
            Console.ResetColor();
        }

        public void ShowPasswordStrength(PasswordStrength strength)
        {
            Console.Write("Password Strength: ");
            switch (strength)
            {
                case PasswordStrength.Weak:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Weak");
                    break;
                case PasswordStrength.Fair:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Fair");
                    break;
                case PasswordStrength.Strong:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Strong");
                    break;
                case PasswordStrength.VeryStrong:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Very Strong");
                    break;
            }
            Console.ResetColor();
        }

        public void ShowCategories(List<CategoryData> categories)
        {
            if (categories == null || categories.Count == 0)
            {
                Console.WriteLine("No categories to display.");
                return;
            }

            Console.WriteLine(new string('-', 45));
            Console.WriteLine($"{"ID",-5} | {"Icon",-6} | {"Category Name",-25}");
            Console.WriteLine(new string('-', 45));

            foreach (var cat in categories)
            {
                Console.WriteLine($"{cat.CategoryDataId,-5} | {cat.Icon ?? "📁",-6} | {cat.CategoryName,-25}");
            }
            Console.WriteLine(new string('-', 45));
        }
    }
}
