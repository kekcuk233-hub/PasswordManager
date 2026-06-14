using PasswordManager.Models.Base;
using PasswordManager.Models.UserData;
using PasswordManager.Services.Utils;

namespace PasswordManager.UI
{
    public class ConsoleDisplayHelper : IDisplayHandler
    {
        public void ShowResult(ResponseMsg result)
        {
            Console.ForegroundColor = result.IsSuccess
                ? ConsoleColor.Green
                : ConsoleColor.Red;

            Console.WriteLine(result.IsSuccess
                ? $"✓ {result.Message}"
                : $"✗ {result.Message}");

            Console.ResetColor();
        }

        public void ShowResult<T>(ResponseMsg<T> result) where T : class
        {
            Console.ForegroundColor = result.IsSuccess
                ? ConsoleColor.Green
                : ConsoleColor.Red;

            Console.WriteLine(result.IsSuccess
                ? $"✓ {result.Message}"
                : $"✗ {result.Message}");

            Console.ResetColor();
        }

        public void ShowEntries(List<CoreDataModel> entries)
        {
            if (entries.Count == 0)
            {
                Console.WriteLine("No entries found.");
                return;
            }

            Console.WriteLine($"\n{"ID",-5} {"Website",-25} {"Email",-30} {"Category",-15} {"Modified",-20}");
            Console.WriteLine(new string('-', 95));

            foreach (var e in entries)
            {
                Console.WriteLine(
                    $"{e.PasswordId,-5} " +
                    $"{Truncate(e.Website, 23),-25} " +
                    $"{Truncate(e.Email, 28),-30} " +
                    $"{Truncate(e.Category?.CategoryName ?? "General", 13),-15} " +
                    $"{e.LastModifiedDate:yyyy-MM-dd HH:mm,-20}");
            }
        }

        public void ShowCategories(List<CategoryData> categories)
        {
            if (categories.Count == 0)
            {
                Console.WriteLine("No categories found.");
                return;
            }

            Console.WriteLine($"\n{"ID",-5} {"Name",-25} {"Icon",-10}");
            Console.WriteLine(new string('-', 40));

            foreach (var c in categories)
                Console.WriteLine($"{c.CategoryDataId,-5} {c.CategoryName,-25} {c.Icon ?? "-",-10}");
        }

        public void ShowPassword(string password)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nPassword: {password}");
            Console.ResetColor();
        }

        public void ShowPasswordStrength(PasswordStrength strength)
        {
            Console.ForegroundColor = strength switch
            {
                PasswordStrength.Weak      => ConsoleColor.Red,
                PasswordStrength.Fair      => ConsoleColor.Yellow,
                PasswordStrength.Strong    => ConsoleColor.Green,
                PasswordStrength.VeryStrong => ConsoleColor.Cyan,
                _                          => ConsoleColor.White
            };

            Console.WriteLine($"Strength: {strength}");
            Console.ResetColor();
        }

        private static string Truncate(string value, int maxLength)
        {
            return value.Length <= maxLength
                ? value
                : value[..(maxLength - 2)] + "..";
        }
    }
}
