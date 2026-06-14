using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using BCrypt.Net;

namespace PasswordManager.Services.Utils
{
    public class CryptoService : ICryptoService
    {
        private readonly UserSession _session;
        // OWASP Recommended Sane Defaults
        private const int MemorySizeKb = 65536; // 64 MB
        private const int Iterations = 3;
        private const int DegreeOfParallelism = 4; // Number of CPU threads to use
        private const int HashLength = 32;        // 256-bit hash

        //BCrypt data
        private const int WorkFactor = 12; 

        public CryptoService(UserSession session)
        {
            _session = session;
        }

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
        public string Encrypt(string plainText)
        {
            byte[] key = _session.GetKey()
                ?? throw new InvalidOperationException("Vault is locked.");

            byte[] iv        = new byte[16];
            RandomNumberGenerator.Fill(iv);

            using var aes     = Aes.Create();
            aes.Key           = key[..32];
            aes.IV            = iv;

            using var encryptor   = aes.CreateEncryptor();
            byte[] plainBytes     = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // Store IV + ciphertext together so Decrypt can recover IV
            byte[] result = new byte[iv.Length + encryptedBytes.Length];
            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(encryptedBytes, 0, result, iv.Length, encryptedBytes.Length);

            return Convert.ToBase64String(result);
        }

        public string Decrypt(string cipherText)
        {
            byte[] key = _session.GetKey()
                ?? throw new InvalidOperationException("Vault is locked.");

            byte[] fullBytes     = Convert.FromBase64String(cipherText);

            // Extract IV from first 16 bytes
            byte[] iv            = fullBytes[..16];
            byte[] encryptedBytes = fullBytes[16..];

            using var aes     = Aes.Create();
            aes.Key           = key[..32];
            aes.IV            = iv;

            using var decryptor   = aes.CreateDecryptor();
            byte[] plainBytes     = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }    
}
