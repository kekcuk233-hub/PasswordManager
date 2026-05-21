using PasswordManager.Models.Base;
using Microsoft.Data.Sqlite;
using PasswordManager.Models.UserData;

namespace PasswordManager.Services
{
    public class UserActions{
        //static private readonly string DataBase = @"DataSource=DataBase/passwords.db";

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
         static public void CreateDb(){
            
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

            // try
            // {
            //     using var connection = new SqliteConnection(DataBase);
            //     connection.Open();

            //     using var command = new SqliteCommand(sql,connection);
            //     command.ExecuteNonQuery();

            //     Console.WriteLine("Table created successfully");

            // }
            // catch (SqliteException ex)
            // {
            //     Console.WriteLine(ex.Message);
            // }
        }

        static public void GetData()
        {
            string getDataSqlCommnd = $"select * from {PasswordTable}";

            // using (var db = new SqliteConnection(DataBase))
            // {
            //     db.Open();
            //     using var command = new SqliteCommand(getDataSqlCommnd, db);

            //     using var reader = command.ExecuteReader();
            //     while (reader.Read())
            //     {
            //         Console.WriteLine($"ID: {reader["id"]} " +
            //       $"Website: {reader["Website"]} " +
            //       $"Email: {reader["Email"]} " + 
            //       $"Password: {reader["Password"]} ");
            //     }
            // }
        }

        static public ResponseMsg AddData(CoreDataModel data)
        {
            if (string.IsNullOrWhiteSpace(data.Password))
            {
                return new ResponseMsg
                {
                    IsSuccess = false,
                    Message = "Please provide password"
                };
            }

            if (string.IsNullOrWhiteSpace(data.Website))
            {
                return new ResponseMsg
                {
                    IsSuccess = false,
                    Message = "Please, provide an Website name"
                };
            }

            if (string.IsNullOrWhiteSpace(data.Email))
            {
                return new ResponseMsg
                {
                    IsSuccess = false,
                    Message = "Please, provide an email"
                };
            }

            var columns = string.Join(", ", DbFields.Select(f => $"[{f}]"));
            var values = string.Join(", ", DbFields.Select(c => $"@{c}"));
            string addNewDataCommand = $"INSERT INTO {PasswordTable}({columns}) VALUES({values})";
            

            // using(var db = new SqliteConnection(DataBase))
            // {
            //     db.Open();

            //     var command = new SqliteCommand(addNewDataCommand, db);

            //     foreach (var field in DbFields)
            //     {
            //         var value = data.GetType().GetProperty(field)?.GetValue(data) ?? DBNull.Value;
            //         Console.WriteLine(value);
            //         command.Parameters.AddWithValue($"@{field}", value);
            //     }

            //     command.ExecuteNonQuery();
            // }

            return new ResponseMsg{IsSuccess = true, Message="Data was added successfully"};
        }
    }
}
