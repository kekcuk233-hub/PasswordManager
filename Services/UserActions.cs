using PasswordManager.Models;
using Microsoft.Data.Sqlite;

namespace PasswordManager.Services
{
    public class UserActions{
        static private readonly string DataBase = @"DataSource=DataBase/passwords.db";
        static public void CreateDb(){
            
            var sql = @"Create Table Passwords(
            id integer Primary Key
            , Website Text
            , Email text not null
            , Password text not null
            , Description
            )";

            try
            {
                using var connection = new SqliteConnection(DataBase);
                connection.Open();

                using var command = new SqliteCommand(sql,connection);
                command.ExecuteNonQuery();

                Console.WriteLine("Table created successfully");

            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static public void GetData()
        {
            string getDataSqlCommnd = @"select * from Passwords";

            using (var db = new SqliteConnection(DataBase))
            {
                db.Open();
                using var command = new SqliteCommand(getDataSqlCommnd, db);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["id"]} " +
                  $"Website: {reader["Website"]} " +
                  $"Email: {reader["Email"]} " + 
                  $"Password: {reader["Password"]} ");
                }
            }
        }

        static public ResponseMsg AddData(UserDataDto data)
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

            string addNewDataCommand = "Insert INTO Passwords(Website, Email, Password, Description)" + 
            "Values(@Website, @Email, @Password, @Description)";

            using(var db = new SqliteConnection(DataBase))
            {
                db.Open();

                var command = new SqliteCommand(addNewDataCommand, db);

                command.Parameters.AddWithValue("@Website", data.Website);
                command.Parameters.AddWithValue("@Email", data.Email);
                command.Parameters.AddWithValue("@Password", data.Password);
                command.Parameters.AddWithValue("@Description", data.Description);

                command.ExecuteNonQuery();
            }

            return new ResponseMsg{IsSuccess = true, Message="Data was added successfully"};
        }
    }
}
