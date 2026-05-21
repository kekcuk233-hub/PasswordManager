using Microsoft.Data.Sqlite;

namespace PasswordManager.DataBase
{
    public class DataBaseContext(string dbPath)
    {
        private readonly string _connectionString = $"DataSource={dbPath}";

        public SqliteConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}
