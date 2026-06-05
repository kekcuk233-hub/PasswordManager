using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using BCrypt.Net;

namespace PasswordManager.Services.Utils
{
    public class CryptoService : ICryptoService
    {
        // OWASP Recommended Sane Defaults
        private const int MemorySizeKb = 65536; // 64 MB
        private const int Iterations = 3;
        private const int DegreeOfParallelism = 4; // Number of CPU threads to use
        private const int HashLength = 32;        // 256-bit hash

        //BCrypt data
        private const int WorkFactor = 12; 

        //Here functions for Argon2Id
        public byte[] DeriveKey(string masterPassword, byte[] salt)
        {
            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(masterPassword));
            argon2.Salt = salt;
            argon2.MemorySize = MemorySizeKb;
            argon2.Iterations = Iterations;
            argon2.DegreeOfParallelism = DegreeOfParallelism;

            byte[] aeskey = argon2.GetBytes(HashLength);

            return aeskey;
        }

        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[32];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }

        //Functions for BCrypt
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        public bool VerifyPassowrd(string password, string storingHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storingHash);
        }
    }    
}
