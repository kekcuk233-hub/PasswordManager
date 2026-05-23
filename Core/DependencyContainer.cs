using PasswordManager.DataBase;
using PasswordManager.DataBase.Repositories;
using PasswordManager.Services;

namespace PasswordManager.Core
{
    public class DependencyContainer
    {
        public DataBaseContext DbContext {get; }
        public IEntryRepository EntryRepo {get; }
        public IVaultService VaultService {get;}

        public DependencyContainer(string dbPath)
        {
            DbContext = new DataBaseContext(dbPath);
            EntryRepo = new EntryRepository(DbContext);
            VaultService = new VaultService(EntryRepo);
        }
    }
}
