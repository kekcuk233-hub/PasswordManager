using System.ComponentModel;
using Microsoft.Data.Sqlite;
using PasswordManager.Models.UserData;

namespace PasswordManager.DataBase
{
    public class DataBaseInitializer(DataBaseContext context)
    {
        private readonly DataBaseContext _context = context;

        public void Initialize()
        {
            using var connection = _context.CreateConnection();
            connection.Open();

            var sql = $@"
                CREATE TABLE IF NOT EXISTS {DbConstants.CategoryTable}(
                    {DbConstants.GetFieldName(DbConstants.CategoryFields.Id)} INTEGER PRIMARY KEY AUTOINCREMENT,
                    {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName)} TEXT,
                    {DbConstants.GetFieldName(DbConstants.CategoryFields.Icon)} TEXT
                );

                CREATE TABLE IF NOT EXISTS {DbConstants.PasswordTable}(
                    {DbConstants.GetFieldName(DbConstants.PasswordFields.Id)} INTEGER PRIMARY KEY AUTOINCREMENT,
                    {DbConstants.GetFieldName(DbConstants.PasswordFields.Website)} TEXT NOT NULL,
                    {DbConstants.GetFieldName(DbConstants.PasswordFields.Email)} TEXT NOT NULL,
                    {DbConstants.GetFieldName(DbConstants.PasswordFields.Password)} TEXT NOT NULL,
                    {DbConstants.GetFieldName(DbConstants.PasswordFields.Url)} TEXT,
                    {DbConstants.GetFieldName(DbConstants.PasswordFields.Description)} TEXT,
                    {DbConstants.GetFieldName(DbConstants.PasswordFields.CategoryId)} INTEGER not null,
                    {DbConstants.GetFieldName(DbConstants.PasswordFields.CreationDate)} TEXT NOT NULL,
                    {DbConstants.GetFieldName(DbConstants.PasswordFields.LastModifiedDate)} TEXT NOT NULL,
                    FOREIGN KEY ({DbConstants.GetFieldName(DbConstants.PasswordFields.CategoryId)})
                    REFERENCES {DbConstants.CategoryTable}({DbConstants.GetFieldName(DbConstants.CategoryFields.Id)})
                );";

            using var command = new SqliteCommand(sql, connection);
            command.ExecuteNonQuery();

            InsertDefaultCategory(connection);
        }
        private void InsertDefaultCategory(SqliteConnection connection)
        {
            try
            {
                var checkSql = $@"
                    SELECT COUNT(*) FROM {DbConstants.CategoryTable} 
                    WHERE {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName)} = 'General'";

                using var checkCommand = new SqliteCommand(checkSql, connection);
                var count = (long)(checkCommand.ExecuteScalar() ?? 0);
                

                if (count == 0)
                {
                    var insertSql = $@"
                        INSERT INTO {DbConstants.CategoryTable}(
                            {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName)}, 
                            {DbConstants.GetFieldName(DbConstants.CategoryFields.Icon)}
                        ) 
                        VALUES('General', '📁')";

                    using var insertCommand = new SqliteCommand(insertSql, connection);
                    insertCommand.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Error inserting default category: {ex.Message}");
            }
        }
    }
}
