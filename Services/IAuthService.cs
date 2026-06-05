using PasswordManager.Models.Base;

namespace PasswordManager.Services
{
    public interface IAuthService
    {
        public ResponseMsg Register(string masterPassword);
        public ResponseMsg Login(string masterPassword);
        public void Logout();
    }
}
