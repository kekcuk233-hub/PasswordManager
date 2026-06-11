using System.Text.Json;
using PasswordManager.DataBase;
using PasswordManager.Models;
using PasswordManager.Models.Base;
using PasswordManager.Services.Utils;

namespace PasswordManager.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserSession _session;
        private readonly ICryptoService _crypto;
        private readonly DataBaseInitializer _dbInit;
        private readonly string _metaPath;

        public AuthService(UserSession session, 
                            ICryptoService crypto,
                            DataBaseInitializer dbInit,
                            string metaPath)
        {
            _session = session;
            _crypto = crypto;
            _dbInit = dbInit;
            _metaPath = metaPath;
        }

        public ResponseMsg Register(string masterPassword)
        {
            if (File.Exists(_metaPath)) return ResponseMsg.Failure("Vault already exists. Please, login");

            if (masterPassword.Length<8) return ResponseMsg.Failure("Master password must be at least 8 characters.");

            byte[] salt = _crypto.GenerateSalt();
            byte[] derivedKey = _crypto.DeriveKey(masterPassword, salt);
            string passwordHash = _crypto.HashPassword(masterPassword);

            _session.SetKey(derivedKey);

            try
            {
                _dbInit.Initialize();

                var meta = new VaultMeta
                {
                    KeySalt = Convert.ToHexString(salt),
                    PasswordHash = passwordHash
                };

                File.WriteAllText(_metaPath, JsonSerializer.Serialize(meta));

                return ResponseMsg.Success("Vault created successfully");
            }
            catch(Exception ex)
            {
                return ResponseMsg.Failure($"Unexpected Error: {ex.Message}"); 
            }
        }
        public ResponseMsg Login(string masterPassword)
        {
            if(!File.Exists(_metaPath))
            {
                return ResponseMsg.Failure("No vault found. Register first");
            }

            var meta = JsonSerializer.Deserialize<VaultMeta>(File.ReadAllText(_metaPath));

            if (meta is null) return ResponseMsg.Failure("Vault metadata was corrupted");

            if(!_crypto.VerifyPassowrd(masterPassword, meta.PasswordHash))
                return ResponseMsg.Failure("Wrong master password");
            
            byte[] salt = Convert.FromHexString(meta.KeySalt);
            
            byte[] derivedKey = _crypto.DeriveKey(masterPassword, salt);

            _session.SetKey(derivedKey);
            
            return ResponseMsg.Success("Logged in successfully");
        }

        public void Logout()
        {
            _session.Lock();
        }
    }
}
