using Microsoft.Data.Sqlite;
using PasswordManager.Models.Base;
using PasswordManager.Models.DTO;
using PasswordManager.Models.UserData;

namespace PasswordManager.DataBase.Repositories
{
    public class EntryRepository : IEntryRepository
    {
        private readonly DataBaseContext _context;

        public EntryRepository(DataBaseContext context)
        {
            _context = context;
        }

        public List<CoreDataModel> GetAll()
        {
            List<CoreDataModel> listData= new();
            using( var db = _context.CreateConnection())
            {
                db.Open();

                var command = db.CreateCommand();
                command.CommandText = $"select * from {DbConstants.PasswordTable}";

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CoreDataModel data = new()
                    {
                        Id = reader.GetInt32(0),
                        Website = reader.GetString(1),
                        Email = reader.GetString(2),
                        Password = reader.GetString(3),
                        Url = reader.IsDBNull(4) ? null : reader.GetString(4),
                        Description = reader.IsDBNull(5) ? null : reader.GetString(5),
                        Category = reader.IsDBNull(6) ? null : reader.GetString(6), //need to fix for real category
                        CreationDate = DateTime.Parse(reader.GetString(7)),
                        LastModifiedDate = DateTime.Parse(reader.GetString(8))
                    };
                    listData.Add(data);
                }
            }
            return listData;
        }

        public ResponseMsg Insert(CoreDataModel data)
        {
            var columns = string.Join(", ", 
                Enum.GetNames<DbConstants.PasswordFields>());
            var values = string.Join(", ", 
                Enum.GetNames<DbConstants.PasswordFields>().Select(n => $"@{n}"));
            string addNewDataCommand = $"Insert into {DbConstants.PasswordTable}({columns}) Values({values})";
            
            using (var db = _context.CreateConnection())
            {
                db.Open();

                var command = new SqliteCommand(addNewDataCommand, db);

                foreach (var field in Enum.GetNames<DbConstants.PasswordFields>())
                {
                    var value = data.GetType().GetProperty(field)?.GetValue(data) ?? DBNull.Value;
                    command.Parameters.AddWithValue($"@{field}", value);
                }

                command.ExecuteNonQuery();
            }
            
            return new ResponseMsg
            {
                IsSuccess = true,
                Message = "Data Was added successfully"
            };
        }

        public ResponseMsg Update(int id, UpdateDto data)
        {
            var updateFields = string.Join(", ", 
            Enum.GetNames<DbConstants.PasswordFields>()
                .Where(f => f != "Id")
                .Where(f=> f != "CreationDate")
                .Select(n => $"{n} = @{n}"));
            string updateDataCommand = $"Update {DbConstants.PasswordTable} Set {updateFields} Where Id = @Id";

            using var db = _context.CreateConnection();
            db.Open();

            var command = new SqliteCommand(updateDataCommand, db);
            command.Parameters.AddWithValue("@Id", id);

            foreach (var field in Enum.GetNames<DbConstants.PasswordFields>().Where(f => f != "Id").Where(f=> f != "CreationDate"))
            {
                var value = data.GetType().GetProperty(field)?.GetValue(data) ?? DBNull.Value;
                command.Parameters.AddWithValue($"@{field}", value);
            }

            int result = command.ExecuteNonQuery();

            if (result == 0)
            {
                return new ResponseMsg
                {
                    IsSuccess = false,
                    Message = "Data was not found"
                };
            }

            return new ResponseMsg
            {
                IsSuccess = true,
                Message = "Data was updated successfully"
            };
        }

        public ResponseMsg Delete(int id)
        {
            return new ResponseMsg();
        }
    }
}
