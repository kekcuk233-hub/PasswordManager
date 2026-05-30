using Microsoft.Data.Sqlite;
using PasswordManager.Models.Base;
using PasswordManager.Models.DTO;
using PasswordManager.Models.UserData;

namespace PasswordManager.DataBase.Repositories
{
    public class EntryRepository : IEntryRepository
    {
        private readonly DataBaseContext _context;
        private static readonly string GetByIdSql = $@"
        SELECT
            p.{DbConstants.GetFieldName(DbConstants.PasswordFields.PasswordId)},
            p.{DbConstants.GetFieldName(DbConstants.PasswordFields.Website)},
            p.{DbConstants.GetFieldName(DbConstants.PasswordFields.Email)},
            p.{DbConstants.GetFieldName(DbConstants.PasswordFields.Password)},
            p.{DbConstants.GetFieldName(DbConstants.PasswordFields.Url)},
            p.{DbConstants.GetFieldName(DbConstants.PasswordFields.Description)},
            p.{DbConstants.GetFieldName(DbConstants.PasswordFields.CategoryId)},
            p.{DbConstants.GetFieldName(DbConstants.PasswordFields.CreationDate)},
            p.{DbConstants.GetFieldName(DbConstants.PasswordFields.LastModifiedDate)},
            c.{DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId)},
            c.{DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName)},
            c.{DbConstants.GetFieldName(DbConstants.CategoryFields.Icon)} 
        FROM {DbConstants.PasswordTable} p
        LEFT JOIN {DbConstants.CategoryTable} c
            ON p.{DbConstants.GetFieldName(DbConstants.PasswordFields.CategoryId)} = 
               c.{DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId)}
        WHERE p.{DbConstants.GetFieldName(DbConstants.PasswordFields.PasswordId)} = {DbConstants.Param(DbConstants.PasswordFields.PasswordId)}";

        private static readonly string GetAllSql = $@"
        SELECT
            p.{DbConstants.GetFieldName(DbConstants.PasswordFields.PasswordId)},
            p.{DbConstants.GetFieldName(DbConstants.PasswordFields.Website)},
            p.{DbConstants.GetFieldName(DbConstants.PasswordFields.Email)},
            p.{DbConstants.GetFieldName(DbConstants.PasswordFields.Password)},
            p.{DbConstants.GetFieldName(DbConstants.PasswordFields.Url)},
            p.{DbConstants.GetFieldName(DbConstants.PasswordFields.Description)},
            p.{DbConstants.GetFieldName(DbConstants.PasswordFields.CategoryId)},
            p.{DbConstants.GetFieldName(DbConstants.PasswordFields.CreationDate)},
            p.{DbConstants.GetFieldName(DbConstants.PasswordFields.LastModifiedDate)},
            c.{DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId)},
            c.{DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName)},
            c.{DbConstants.GetFieldName(DbConstants.CategoryFields.Icon)} 
        FROM {DbConstants.PasswordTable} p
        LEFT JOIN {DbConstants.CategoryTable} c
            ON p.{DbConstants.GetFieldName(DbConstants.PasswordFields.CategoryId)} = 
               c.{DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId)}"; 
        
        private static readonly string InsertSql = $@"
        INSERT INTO {DbConstants.PasswordTable} (
            {DbConstants.GetFieldName(DbConstants.PasswordFields.Website)},
            {DbConstants.GetFieldName(DbConstants.PasswordFields.Email)},
            {DbConstants.GetFieldName(DbConstants.PasswordFields.Password)},
            {DbConstants.GetFieldName(DbConstants.PasswordFields.Url)},
            {DbConstants.GetFieldName(DbConstants.PasswordFields.Description)},
            {DbConstants.GetFieldName(DbConstants.PasswordFields.CategoryId)},
            {DbConstants.GetFieldName(DbConstants.PasswordFields.CreationDate)},
            {DbConstants.GetFieldName(DbConstants.PasswordFields.LastModifiedDate)}
        ) VALUES (
            {DbConstants.Param(DbConstants.PasswordFields.Website)},
            {DbConstants.Param(DbConstants.PasswordFields.Email)},
            {DbConstants.Param(DbConstants.PasswordFields.Password)},
            {DbConstants.Param(DbConstants.PasswordFields.Url)},
            {DbConstants.Param(DbConstants.PasswordFields.Description)},
            {DbConstants.Param(DbConstants.PasswordFields.CategoryId)},
            {DbConstants.Param(DbConstants.PasswordFields.CreationDate)},
            {DbConstants.Param(DbConstants.PasswordFields.LastModifiedDate)}
        )";

        private static readonly string UpdateSql = $@"
        UPDATE {DbConstants.PasswordTable}
        SET
            {DbConstants.GetFieldName(DbConstants.PasswordFields.Website)} = {DbConstants.Param(DbConstants.PasswordFields.Website)},
            {DbConstants.GetFieldName(DbConstants.PasswordFields.Email)} = {DbConstants.Param(DbConstants.PasswordFields.Email)},
            {DbConstants.GetFieldName(DbConstants.PasswordFields.Password)} = {DbConstants.Param(DbConstants.PasswordFields.Password)},
            {DbConstants.GetFieldName(DbConstants.PasswordFields.Url)} = {DbConstants.Param(DbConstants.PasswordFields.Url)},
            {DbConstants.GetFieldName(DbConstants.PasswordFields.Description)} = {DbConstants.Param(DbConstants.PasswordFields.Description)},
            {DbConstants.GetFieldName(DbConstants.PasswordFields.CategoryId)} = {DbConstants.Param(DbConstants.PasswordFields.CategoryId)},
            {DbConstants.GetFieldName(DbConstants.PasswordFields.LastModifiedDate)} = {DbConstants.Param(DbConstants.PasswordFields.LastModifiedDate)}
        WHERE {DbConstants.GetFieldName(DbConstants.PasswordFields.PasswordId)} = {DbConstants.Param(DbConstants.PasswordFields.PasswordId)}";

        private static readonly string DeleteSql = $@"
            Delete from {DbConstants.PasswordTable} where 
            {DbConstants.GetFieldName(DbConstants.PasswordFields.PasswordId)} = {DbConstants.Param(DbConstants.PasswordFields.PasswordId)}";
        
        private static readonly string ReassignSql = $@"
            Update {DbConstants.PasswordTable}
            set {DbConstants.GetFieldName(DbConstants.PasswordFields.CategoryId)} = 
                {DbConstants.Param(DbConstants.PasswordFields.CategoryId)}
            where {DbConstants.GetFieldName(DbConstants.PasswordFields.CategoryId)} = 
                @OldCategoryId
            ";

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
                    var command = db.CreateCommand();
                    command.CommandText = GetAllSql;

                    using var reader = command.ExecuteReader();

                    int passwordIdOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.PasswordFields.PasswordId));
                    int websiteOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.PasswordFields.Website));
                    int emailOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.PasswordFields.Email));
                    int passwordOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.PasswordFields.Password));
                    int urlOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.PasswordFields.Url));
                    int descriptionOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.PasswordFields.Description));
                    int categoryIdOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.PasswordFields.CategoryId));
                    int creationDateOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.PasswordFields.CreationDate));
                    int lastModifiedDateOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.PasswordFields.LastModifiedDate));
                    int categoryDataIdOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId));
                    int categoryNameOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName));
                    int categoryIconOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.CategoryFields.Icon));
                    
                    while (reader.Read())
                    {
                        CoreDataModel data = new()
                        {
                            PasswordId = reader.GetInt32(passwordIdOrdinal),
                            Website = reader.GetString(websiteOrdinal),
                            Email = reader.GetString(emailOrdinal),
                            Password = reader.GetString(passwordOrdinal),
                            Url = reader.IsDBNull(urlOrdinal) ? null : reader.GetString(urlOrdinal),
                            Description = reader.IsDBNull(descriptionOrdinal) ? null : reader.GetString(descriptionOrdinal),
                            CategoryId = reader.IsDBNull(categoryIdOrdinal) ? null : reader.GetInt32(categoryIdOrdinal),
                            Category = reader.IsDBNull(categoryDataIdOrdinal) ? null : new CategoryData
                            {
                                CategoryDataId = reader.GetInt32(categoryDataIdOrdinal),
                                CategoryName = reader.GetString(categoryNameOrdinal),
                                Icon = reader.IsDBNull(categoryIconOrdinal) ? null : reader.GetString(categoryIconOrdinal)
                            },
                            CreationDate = DateTime.Parse(reader.GetString(creationDateOrdinal)),
                            LastModifiedDate = DateTime.Parse(reader.GetString(lastModifiedDateOrdinal))
                        };
                        listData.Add(data);
                    }
                    return new ResponseMsg<List<CoreDataModel>>
                    {
                        IsSuccess = true,
                        Message = "Data retrieved successfully",
                        Data = listData
                    };
                }
            }
            catch(SqliteException ex)
            {
                return new ResponseMsg<List<CoreDataModel>>
                {
                    IsSuccess = false,
                    Message = $"DataBase error: {ex.Message}"
                };
            }
            catch(Exception ex)
            {
                return new ResponseMsg<List<CoreDataModel>>
                {
                    IsSuccess = false,
                    Message = $"Unexpected error: {ex.Message}"
                };
            }
        }

        public ResponseMsg<CoreDataModel> GetById(int id)
        {
            try
            {
                using var db = _context.CreateConnection();

                var command = db.CreateCommand();
                command.CommandText = GetByIdSql;
                
                command.Parameters.AddWithValue(DbConstants.Param(DbConstants.PasswordFields.PasswordId), id);

                using var reader = command.ExecuteReader();

                if(reader.Read())
                {
                    int passwordIdOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.PasswordFields.PasswordId));
                    int websiteOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.PasswordFields.Website));
                    int emailOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.PasswordFields.Email));
                    int passwordOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.PasswordFields.Password));
                    int urlOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.PasswordFields.Url));
                    int descriptionOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.PasswordFields.Description));
                    int categoryIdOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.PasswordFields.CategoryId));
                    int creationDateOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.PasswordFields.CreationDate));
                    int lastModifiedDateOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.PasswordFields.LastModifiedDate));
                    int categoryDataIdOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId));
                    int categoryNameOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName));
                    int categoryIconOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.CategoryFields.Icon));

                    CoreDataModel data = new()
                    {
                        PasswordId = reader.GetInt32(passwordIdOrdinal),
                        Website = reader.GetString(websiteOrdinal),
                        Email = reader.GetString(emailOrdinal),
                        Password = reader.GetString(passwordOrdinal),
                        Url = reader.IsDBNull(urlOrdinal) ? null : reader.GetString(urlOrdinal),
                        Description = reader.IsDBNull(descriptionOrdinal) ? null : reader.GetString(descriptionOrdinal),
                        CategoryId = reader.IsDBNull(categoryIdOrdinal) ? null : reader.GetInt32(categoryIdOrdinal),
                        Category = reader.IsDBNull(categoryDataIdOrdinal) ? null : new CategoryData
                        {
                            CategoryDataId = reader.GetInt32(categoryDataIdOrdinal),
                            CategoryName = reader.GetString(categoryNameOrdinal),
                            Icon = reader.IsDBNull(categoryIconOrdinal) ? null : reader.GetString(categoryIconOrdinal)
                        },
                        CreationDate = DateTime.Parse(reader.GetString(creationDateOrdinal)),
                        LastModifiedDate = DateTime.Parse(reader.GetString(lastModifiedDateOrdinal))
                    };

                    return new ResponseMsg<CoreDataModel>
                    {
                        IsSuccess = true,
                        Message = $"Entry with ID {id} retrieved successfully",
                        Data = data
                    };
                }
                else
                {
                    return new ResponseMsg<CoreDataModel>
                    {
                        IsSuccess = false,
                        Message = $"Entry with ID {id} was not found"
                    };
                }
                
            }
            catch(SqliteException ex)
            {
                return new ResponseMsg<CoreDataModel>
                {
                    IsSuccess = false, 
                    Message = $"DataBase Error: {ex.Message}"
                };
            }
            catch(Exception ex)
            {
                return new ResponseMsg<CoreDataModel>
                {
                    IsSuccess = false,
                    Message = $"Unexcpected Error occurred: {ex.Message}"
                };
            }
        }
        public ResponseMsg<CoreDataModel> Insert(CoreDataModel data)
        {
            try
            {   
                using var db = _context.CreateConnection();

                var command = db.CreateCommand();
                command.CommandText = InsertSql;

                command.Parameters.AddWithValue(DbConstants.Param(DbConstants.PasswordFields.Website), data.Website);
                command.Parameters.AddWithValue(DbConstants.Param(DbConstants.PasswordFields.Email), data.Email);
                command.Parameters.AddWithValue(DbConstants.Param(DbConstants.PasswordFields.Password), data.Password);
                command.Parameters.AddWithValue(DbConstants.Param(DbConstants.PasswordFields.Url), data.Url ?? (object)DBNull.Value);
                command.Parameters.AddWithValue(DbConstants.Param(DbConstants.PasswordFields.Description), data.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue(DbConstants.Param(DbConstants.PasswordFields.CategoryId), data.CategoryId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue(DbConstants.Param(DbConstants.PasswordFields.CreationDate), data.CreationDate.ToString("o"));
                command.Parameters.AddWithValue(DbConstants.Param(DbConstants.PasswordFields.LastModifiedDate), data.LastModifiedDate.ToString("o"));

                command.ExecuteNonQuery();
                
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
            catch(Exception ex)
            {
                return new ResponseMsg<CoreDataModel>
                {
                    IsSuccess = false,
                    Message = $"Unexcpected Error occurred: {ex.Message}"
                };
            }
        }
        public ResponseMsg<CoreDataModel> Update(int id, UpdateDto data)
        {
            try
            {
                using var db = _context.CreateConnection();

                var command = db.CreateCommand();
                command.CommandText = UpdateSql;
                command.Parameters.AddWithValue($"{DbConstants.Param(DbConstants.PasswordFields.PasswordId)}", id);

                command.Parameters.AddWithValue(DbConstants.Param(DbConstants.PasswordFields.Website), data.Website);
                command.Parameters.AddWithValue(DbConstants.Param(DbConstants.PasswordFields.Email), data.Email);
                command.Parameters.AddWithValue(DbConstants.Param(DbConstants.PasswordFields.Password), data.Password);
                command.Parameters.AddWithValue(DbConstants.Param(DbConstants.PasswordFields.Url), data.Url ?? (object)DBNull.Value);
                command.Parameters.AddWithValue(DbConstants.Param(DbConstants.PasswordFields.Description), data.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue(DbConstants.Param(DbConstants.PasswordFields.CategoryId), data.CategoryId ?? (object)DBNull.Value);
                command.Parameters.AddWithValue(DbConstants.Param(DbConstants.PasswordFields.LastModifiedDate), DateTime.Now.ToString("o"));

                command.ExecuteNonQuery();

                return new ResponseMsg<CoreDataModel>
                {
                    IsSuccess = true,
                    Message = "Data was updated successfully"
                };
            }
            catch(SqliteException ex)
            {
                return new ResponseMsg<CoreDataModel>
                {
                    IsSuccess = false,
                    Message = $"DataBase Error: {ex.Message}"
                };
            }
            catch(Exception ex)
            {
                return new ResponseMsg<CoreDataModel>
                {
                    IsSuccess = false,
                    Message = $"Unexcpected Error occurred: {ex.Message}"
                };
            }
        }

        public ResponseMsg<CoreDataModel> Delete(int id)
        {
            try 
            {
                using var db = _context.CreateConnection();

                var command = db.CreateCommand();
                command.CommandText = DeleteSql;
                command.Parameters.AddWithValue($"{DbConstants.Param(DbConstants.PasswordFields.PasswordId)}", id);

                command.ExecuteNonQuery();

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
            catch(Exception ex)
            {
                return new ResponseMsg<CoreDataModel>
                {
                    IsSuccess = false,
                    Message = $"Unexcpected Error occurred: {ex.Message}"
                };
            }
        }

        public ResponseMsg<CoreDataModel> ReassignCategory(int id, int defaultId)
        {
            try
            {
                var db = _context.CreateConnection();

                using var command = new SqliteCommand(ReassignSql, db);
                command.Parameters.AddWithValue(
                    DbConstants.Param(DbConstants.CategoryFields.CategoryDataId), defaultId
                );
                command.Parameters.AddWithValue("@OldCategoryId", id);

                command.ExecuteNonQuery();

                return new ResponseMsg<CoreDataModel>
                {
                    IsSuccess = true,
                    Message = "Data was updated successfully"
                };
            }
            catch(SqliteException ex)
            {
                return new ResponseMsg<CoreDataModel>
                {
                    IsSuccess = false,
                    Message = $"DataBase Error: {ex.Message}"
                };
            }
            catch(Exception ex)
            {
                return new ResponseMsg<CoreDataModel>
                {
                    IsSuccess = false,
                    Message = $"Unexcpected Error occurred: {ex.Message}"
                };
            }
        }
    }
}
