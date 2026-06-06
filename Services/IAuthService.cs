using PasswordManager.Models.Base;

namespace PasswordManager.Services
{
    public interface IAuthService
    {
        ResponseMsg Register(string masterPassword);
        ResponseMsg Login(string masterPassword);
        void Logout();
    }
}
