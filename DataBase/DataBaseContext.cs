using Microsoft.Data.Sqlite;
using PasswordManager.Models;

namespace PasswordManager.DataBase
{
    public class DataBaseContext(string dbPath, UserSession session)
    {
        private readonly string _dbPath = dbPath;
        private readonly UserSession _session = session;

        public SqliteConnection CreateConnection()
        {
            byte[] key = _session.GetKey() ?? throw new Exception("Vault is locked! Please login first.");
            
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = _dbPath,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Password = Convert.ToHexString(key)
            };

            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    PRAGMA foreign_keys = ON;
                    PRAGMA journal_mode = WAL;
                    PRAGMA cipher_kdf_algorithm = 'PBKDF2_HMAC_SHA256';
                    PRAGMA cipher_page_size = 4096;
                ";
                command.ExecuteNonQuery();
            }

            return connection;
        }
    }
}
