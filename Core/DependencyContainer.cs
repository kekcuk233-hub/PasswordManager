using PasswordManager.DataBase;

namespace PasswordManager.Core
{
    public class DependencyContainer
    {
        public DataBaseContext DbContext {get; }

        public DependencyContainer(string dbPath)
        {
            DbContext = new DataBaseContext(dbPath);
        }
    }
}
