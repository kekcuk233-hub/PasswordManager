using Microsoft.Data.Sqlite;

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
                Password = Convert.ToHexString(key),
            };

            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    PRAGMA cipher                = 'aes256cbc';
                    PRAGMA kdf_algorithm         = PBKDF2_HMAC_SHA512;
                    PRAGMA kdf_iter              = 256000;
                    PRAGMA cipher_page_size      = 4096;
                    PRAGMA cipher_hmac_algorithm = HMAC_SHA512;
                    PRAGMA foreign_keys          = ON;
                    PRAGMA journal_mode          = WAL;
                ";
                command.ExecuteNonQuery();
            }

            return connection;
        }
    }
}
