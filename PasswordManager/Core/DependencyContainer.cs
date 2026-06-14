using PasswordManager.DataBase;
using PasswordManager.DataBase.Repositories;
using PasswordManager.Services;
using PasswordManager.Services.Utils;
using PasswordManager.UI;

namespace PasswordManager.Core
{
    public class DependencyContainer
    {
        public IVaultService             VaultService     { get; }
        public ICryptoService            CryptoService    { get; }
        public IPasswordGeneratorService PasswordGenerator { get; }
        public IAuthService              AuthService      { get; }
        public IDisplayHandler           Display          { get; }
        public IMenuHandler              Menu             { get; }

        public DependencyContainer(string dbPath, string metaPath)
        {
            var userSession   = new UserSession();
            var dbContext     = new DataBaseContext(dbPath, userSession);
            var entryRepo     = new EntryRepository(dbContext);
            var categoryRepo  = new CategoryRepository(dbContext);
            var dbInit        = new DataBaseInitializer(dbContext);

            CryptoService     = new CryptoService(userSession);
            PasswordGenerator = new PasswordGeneratorService();
            AuthService       = new AuthService(userSession, CryptoService, dbInit, metaPath);
            VaultService      = new VaultService(entryRepo, categoryRepo, CryptoService, userSession);
            Display           = new ConsoleDisplayHelper();
            Menu              = new ConsoleMenu();
        }
    }
}
