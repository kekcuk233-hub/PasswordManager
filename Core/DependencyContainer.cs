using PasswordManager.DataBase;
using PasswordManager.DataBase.Repositories;
using PasswordManager.Services;
using PasswordManager.Services.Utils;

namespace PasswordManager.Core
{
    public class DependencyContainer
    {
        public UserSession UserSession { get; }
        public DataBaseContext DbContext {get; }
        public IEntryRepository EntryRepo {get; }
        public ICategoryRepository CategoryRepo {get; }
        public IVaultService VaultService {get;}
        public ICryptoService CryptoService{get; }
        public IPasswordGeneratorService PasswordGenerator {get; }
        public DataBaseInitializer DbInit {get; }
        public IAuthService AuthService {get; }

        public DependencyContainer(string dbPath, string metaPath)
        {
            UserSession = new UserSession();
            DbContext = new DataBaseContext(dbPath, UserSession);
            EntryRepo = new EntryRepository(DbContext);
            CategoryRepo = new CategoryRepository(DbContext);
            CryptoService = new CryptoService();
            PasswordGenerator = new PasswordGeneratorService();
            DbInit = new DataBaseInitializer(DbContext);
            AuthService = new AuthService(UserSession, CryptoService, DbInit, metaPath);
            VaultService = new VaultService(EntryRepo, CategoryRepo, CryptoService, UserSession);
        }
    }
}
