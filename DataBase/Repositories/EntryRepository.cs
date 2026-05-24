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

        public ResponseMsg<List<CoreDataModel>> GetAll()
        {
            List<CoreDataModel> listData= [];
            try {
                using( var db = _context.CreateConnection())
                {
                    db.Open();

                    var command = db.CreateCommand();
                    command.CommandText = $@"select * from {DbConstants.PasswordTable} p 
                    Left join {DbConstants.CategoryTable} c ON 
                    p.{DbConstants.GetFieldName(DbConstants.PasswordFields.CategoryId)} = 
                    c.{DbConstants.GetFieldName(DbConstants.CategoryFields.Id)}";

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
                            CategoryId = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                            Category = reader.IsDBNull(9) ? null : new CategoryData
                            {
                                Id = reader.GetInt32(9),
                                CategoryName = reader.GetString(10),
                                Icon = reader.IsDBNull(11) ? null : reader.GetString(11)
                            },
                            CreationDate = DateTime.Parse(reader.GetString(7)),
                            LastModifiedDate = DateTime.Parse(reader.GetString(8))
                        };
                        listData.Add(data);
                    }
                }
                return new ResponseMsg<List<CoreDataModel>>
                {
                    IsSuccess = true,
                    Message = "Data retrieved successfully",
                    Data = listData
                };
            }
            catch(SqliteException ex)
            {
                return new ResponseMsg<List<CoreDataModel>>
                {
                    IsSuccess = false,
                    Message = $"DataBase error: {ex.Message}"
                };
            }
        }

        public ResponseMsg<CoreDataModel> Insert(CoreDataModel data)
        {
            try
            {
                var columns = string.Join(", ", 
                    Enum.GetNames<DbConstants.PasswordFields>()
                    .Where(f => f != "Id")
                    .Select(n => $"{n}"));
                var values = string.Join(", ", 
                    Enum.GetNames<DbConstants.PasswordFields>()
                    .Where(f => f != "Id")
                    .Select(n => $"@{n}"));
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
                
                return new ResponseMsg<CoreDataModel>
                {
                    IsSuccess = true,
                    Message = "Data Was added successfully"
                };
            }
            catch(SqliteException ex)
            {
                return new ResponseMsg<CoreDataModel>
                {
                    IsSuccess = false,
                    Message = $"DataBase Error : {ex.Message}"
                };
            }
        }

        public ResponseMsg<CoreDataModel> Update(int id, UpdateDto data)
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
                return new ResponseMsg<CoreDataModel>
                {
                    IsSuccess = false,
                    Message = "Data was not found"
                };
            }

            return new ResponseMsg<CoreDataModel>
            {
                IsSuccess = true,
                Message = "Data was updated successfully"
            };
        }

        public ResponseMsg<CoreDataModel> Delete(int id)
        {
            string deleteUserData = $"Delete from {DbConstants.PasswordTable} where Id = @Id";
            using var db = _context.CreateConnection();

            try 
            {
                db.Open();
                var command = new SqliteCommand(deleteUserData, db);
                command.Parameters.AddWithValue("@Id", id);
                var result = command.ExecuteNonQuery();

                return new ResponseMsg<CoreDataModel>
                {
                    IsSuccess = true,
                    Message = "Data was deleted successfully"
                };
            }
            catch (SqliteException ex)
            {
                return new ResponseMsg<CoreDataModel>
                {
                    IsSuccess = false,
                    Message = $"DataBase Error : {ex.Message}"
                };
            }
        }
    }
}
