namespace PasswordManager.Services.Utils
{
    public interface ICryptoService
    {
        //For Argon2Id
        byte[] GenerateSalt();
        byte[] DeriveKey(string masterPassword, byte[] salt);

        //For BCrypt
        string HashPassword(string masterPassword);
        bool VerifyPassowrd(string password, string storingHash);
    }
}
