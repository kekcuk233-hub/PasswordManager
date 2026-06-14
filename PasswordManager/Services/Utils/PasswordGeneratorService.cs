using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Services.Utils
{
    public class PasswordGeneratorService : IPasswordGeneratorService
    {
        private const string Lowercase  = "abcdefghijklmnopqrstuvwxyz";
        private const string Uppercase  = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Digits     = "0123456789";
        private const string Symbols    = "!@#$%^&*()_+-=[]{}|;:,.<>?";

        public record PasswordOptions
        {
            public int  Length         { get; init; } = 16;
            public bool UseLowercase   { get; init; } = true;
            public bool UseUppercase   { get; init; } = true;
            public bool UseDigits      { get; init; } = true;
            public bool UseSymbols     { get; init; } = true;
        }
        public string Generate(PasswordOptions options)
        {
            var pool = BuildPool(options);

            if (pool.Length == 0)
                throw new ArgumentException("At least one character set must be selected.");

            if (options.Length < 4)
                throw new ArgumentException("Password length must be at least 4.");

            string password;
            int maxAttempts = 100;

            do
            {
                password = GenerateRaw(pool, options.Length);
                maxAttempts--;
            }
            while (!MeetsRequirements(password, options) && maxAttempts > 0);

            return password;
        }

        private static string BuildPool(PasswordOptions options)
        {
            var pool = new StringBuilder();

            if (options.UseLowercase) pool.Append(Lowercase);
            if (options.UseUppercase) pool.Append(Uppercase);
            if (options.UseDigits)    pool.Append(Digits);
            if (options.UseSymbols)   pool.Append(Symbols);

            return pool.ToString();
        }

        private static string GenerateRaw(string pool, int length)
        {
            var password = new StringBuilder();
            int limit    = 256 - (256 % pool.Length);
            Span<byte> buffer = stackalloc byte[1];

            for (int i = 0; i < length; i++)
            {
                byte randomByte;
                do
                {
                    RandomNumberGenerator.Fill(buffer);
                    randomByte = buffer[0];
                }
                while (randomByte >= limit);

                password.Append(pool[randomByte % pool.Length]);
            }

            return password.ToString();
        }

        // Ensure at least one character from each required set is present
        private static bool MeetsRequirements(string password, PasswordOptions options)
        {
            if (options.UseLowercase && !password.Any(c => Lowercase.Contains(c))) return false;
            if (options.UseUppercase && !password.Any(c => Uppercase.Contains(c))) return false;
            if (options.UseDigits    && !password.Any(c => Digits.Contains(c)))    return false;
            if (options.UseSymbols   && !password.Any(c => Symbols.Contains(c)))   return false;

            return true;
        }

        public PasswordStrength CheckStrength(string password)
        {
            int score = 0;

            if (password.Length >= 8)  score++;
            if (password.Length >= 12) score++;
            if (password.Length >= 16) score++;

            if (password.Any(c => Lowercase.Contains(c)))  score++;
            if (password.Any(c => Uppercase.Contains(c)))  score++;
            if (password.Any(c => Digits.Contains(c)))     score++;
            if (password.Any(c => Symbols.Contains(c)))    score++;

            // Penalise repetition — more than 3 identical chars in a row
            if (System.Text.RegularExpressions.Regex.IsMatch(password, @"(.)\1{2,}"))
                score--;

            return score switch
            {
                <= 2 => PasswordStrength.Weak,
                <= 4 => PasswordStrength.Fair,
                <= 6 => PasswordStrength.Strong,
                _    => PasswordStrength.VeryStrong
            };
        }
        public bool TryGenerate(PasswordOptions options, out string? password, out string? error)
        {
            try
            {
                password = Generate(options);
                error    = null;
                return true;
            }
            catch (ArgumentException ex)
            {
                password = null;
                error    = ex.Message;
                return false;
            }
        }
    }

    public enum PasswordStrength
    {
        Weak,
        Fair,
        Strong,
        VeryStrong
    }
}
