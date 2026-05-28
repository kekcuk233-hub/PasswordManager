using Microsoft.Data.Sqlite;

namespace PasswordManager.DataBase
{
    public class DataBaseInitializer(DataBaseContext context)
    {
        private readonly DataBaseContext _context = context;

        private static readonly string sql = $@"
            CREATE TABLE IF NOT EXISTS {DbConstants.CategoryTable}(
                {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId)} INTEGER PRIMARY KEY AUTOINCREMENT,
                {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName)} TEXT not null Unique,
                {DbConstants.GetFieldName(DbConstants.CategoryFields.Icon)} TEXT
            );

            CREATE TABLE IF NOT EXISTS {DbConstants.PasswordTable}(
                {DbConstants.GetFieldName(DbConstants.PasswordFields.PasswordId)} INTEGER PRIMARY KEY AUTOINCREMENT,
                {DbConstants.GetFieldName(DbConstants.PasswordFields.Website)} TEXT NOT NULL,
                {DbConstants.GetFieldName(DbConstants.PasswordFields.Email)} TEXT NOT NULL,
                {DbConstants.GetFieldName(DbConstants.PasswordFields.Password)} TEXT NOT NULL,
                {DbConstants.GetFieldName(DbConstants.PasswordFields.Url)} TEXT,
                {DbConstants.GetFieldName(DbConstants.PasswordFields.Description)} TEXT,
                {DbConstants.GetFieldName(DbConstants.PasswordFields.CategoryId)} INTEGER not null,
                {DbConstants.GetFieldName(DbConstants.PasswordFields.CreationDate)} TEXT NOT NULL,
                {DbConstants.GetFieldName(DbConstants.PasswordFields.LastModifiedDate)} TEXT NOT NULL,
                FOREIGN KEY ({DbConstants.GetFieldName(DbConstants.PasswordFields.CategoryId)})
                REFERENCES {DbConstants.CategoryTable}({DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId)})
            );";
        private static readonly string insertDefaultSql = $@"
        INSERT OR IGNORE INTO {DbConstants.CategoryTable}(
                            {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName)}, 
                            {DbConstants.GetFieldName(DbConstants.CategoryFields.Icon)}
                        ) 
                        VALUES(
                            {DbConstants.Param(DbConstants.CategoryFields.CategoryName)}, 
                            {DbConstants.Param(DbConstants.CategoryFields.Icon)}
                        )";

        public void Initialize()
        {
            try
            {
                using var connection = _context.CreateConnection();

                using var command = new SqliteCommand(sql, connection);
                command.ExecuteNonQuery();

                InsertDefaultCategory(connection);
            }
            catch(SqliteException ex)
            {
                throw new Exception($"Failed to initialize database: {ex.Message}", ex);
            }
        }
        private void InsertDefaultCategory(SqliteConnection connection)
        {
                using var insertCommand = new SqliteCommand(insertDefaultSql, connection);

                insertCommand.Parameters.AddWithValue(
                    DbConstants.Param(DbConstants.CategoryFields.CategoryName), DbConstants.DefaultCategoryName);
                insertCommand.Parameters.AddWithValue(
                    DbConstants.Param(DbConstants.CategoryFields.Icon), (object)DBNull.Value);

                insertCommand.ExecuteNonQuery();
        }
    }
}
