using Microsoft.Data.Sqlite;

namespace PasswordManager.DataBase
{
    public class DataBaseInitializer(DataBaseContext context)
    {
        private readonly DataBaseContext _context = context;
        static private readonly string PasswordTable = "Passwords";
        static private readonly List<string> DbFields = new List<string>
        {
            "Website",
            "Email",
            "Password",
            "Url",
            "Description",
            "Category",
            "CreationDate",
            "LastModifiedDate"
        };

        public void Initialize()
        {
            using var connection = _context.CreateConnection();
            connection.Open();

            var sql = $@"Create Table Passwords(
            id integer Primary Key,
            {DbFields[0]} text not null,
            {DbFields[1]} text not null,
            {DbFields[2]} text not null,
            {DbFields[3]} text,
            {DbFields[4]} text,
            {DbFields[5]} text,
            {DbFields[6]} text,
            {DbFields[7]} text
            )";
            // Foreign Key (CategoryId) References Categories(Id) for category when I add it

            using var command = new SqliteCommand(sql, connection);
            command.ExecuteNonQuery();
        }
    }
}
