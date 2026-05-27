using Microsoft.Data.Sqlite;

namespace PasswordManager.DataBase
{
    public class DataBaseContext(string dbPath)
    {
        private readonly string _connectionString = $"DataSource={dbPath}";

        public SqliteConnection CreateConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var fkCmd = new SqliteCommand("PRAGMA foreign_keys = ON;", connection);
            fkCmd.ExecuteNonQuery();

            using var walCmd = new SqliteCommand("PRAGMA journal_mode = WAL;", connection);
            walCmd.ExecuteNonQuery();

            return connection;
        }
    }
}
