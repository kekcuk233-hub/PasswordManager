namespace PasswordManager.Models
{
    public class VaultMeta
    {
        public string KeySalt {get; set;} = string.Empty;
        public string PasswordHash {get; set;} = string.Empty;
    }
}
