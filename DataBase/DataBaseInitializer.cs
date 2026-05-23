using Microsoft.Data.Sqlite;

namespace PasswordManager.DataBase
{
    public class DataBaseInitializer(DataBaseContext context)
    {
        private readonly DataBaseContext _context = context;

        public void Initialize()
        {
            using var connection = _context.CreateConnection();
            connection.Open();

            var sql = $@"Create Table if not Exists {DbConstants.PasswordTable}(
            Id integer Primary Key,
            {DbConstants.GetFieldName(DbConstants.PasswordFields.Website)} text not null,
            {DbConstants.GetFieldName(DbConstants.PasswordFields.Email)} text not null,
            {DbConstants.GetFieldName(DbConstants.PasswordFields.Password)} text not null,
            {DbConstants.GetFieldName(DbConstants.PasswordFields.Url)} text,
            {DbConstants.GetFieldName(DbConstants.PasswordFields.Description)} text,
            {DbConstants.GetFieldName(DbConstants.PasswordFields.Category)} text,
            {DbConstants.GetFieldName(DbConstants.PasswordFields.CreationDate)} text,
            {DbConstants.GetFieldName(DbConstants.PasswordFields.LastModifiedDate)} text
            )";
            // Foreign Key (CategoryId) References Categories(Id) for category when I add it

            using var command = new SqliteCommand(sql, connection);
            command.ExecuteNonQuery();
        }
    }
}
