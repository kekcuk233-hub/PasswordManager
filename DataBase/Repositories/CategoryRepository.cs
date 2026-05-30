using Microsoft.Data.Sqlite;
using PasswordManager.Models.Base;
using PasswordManager.Models.DTO;
using PasswordManager.Models.UserData;

namespace PasswordManager.DataBase.Repositories
{
    public class CategoryRepository(DataBaseContext context) : ICategoryRepository
    {
        private readonly DataBaseContext _context = context;

        private static readonly string GetByIdSql = $@"
            select
                {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId)},
                {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName)},
                {DbConstants.GetFieldName(DbConstants.CategoryFields.Icon)}
            from {DbConstants.CategoryTable} 
            where {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId)} = {DbConstants.Param(DbConstants.CategoryFields.CategoryDataId)}";

        private static readonly string GetByNameSql = $@"
            select
                {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId)},
                {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName)},
                {DbConstants.GetFieldName(DbConstants.CategoryFields.Icon)}
            from {DbConstants.CategoryTable} 
            where {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName)} = {DbConstants.Param(DbConstants.CategoryFields.CategoryName)}";

        private static readonly string AddSql = $@"
            insert into {DbConstants.CategoryTable}(
                {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName)},
                {DbConstants.GetFieldName(DbConstants.CategoryFields.Icon)}
            ) Values (
                {DbConstants.Param(DbConstants.CategoryFields.CategoryName)},
                {DbConstants.Param(DbConstants.CategoryFields.Icon)}
            )";

        private static readonly string DeleteSql = $@"
            delete from {DbConstants.CategoryTable} where 
            {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId)} = {DbConstants.Param(DbConstants.CategoryFields.CategoryDataId)}";

        private static readonly string GetAllSql = $@"
            select 
                {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId)},
                {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName)},
                {DbConstants.GetFieldName(DbConstants.CategoryFields.Icon)}
            from {DbConstants.CategoryTable}
            ";

        private static readonly string UpdateSql = $@"
            update {DbConstants.CategoryTable} set 
                {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName)} = {DbConstants.Param(DbConstants.CategoryFields.CategoryName)},
                {DbConstants.GetFieldName(DbConstants.CategoryFields.Icon)} = {DbConstants.Param(DbConstants.CategoryFields.Icon)}
            where 
                {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId)} = {DbConstants.Param(DbConstants.CategoryFields.CategoryDataId)}
            ";

        public ResponseMsg<CategoryData> Add(CategoryData data)
        {
            try
            {
                using var db = _context.CreateConnection();

                using var command = new SqliteCommand(AddSql, db);
                command.Parameters.AddWithValue(
                    DbConstants.Param(DbConstants.CategoryFields.CategoryName), data.CategoryName
                );
                command.Parameters.AddWithValue(
                    DbConstants.Param(DbConstants.CategoryFields.Icon), data.Icon
                );

                command.ExecuteNonQuery();

                return new ResponseMsg<CategoryData>
                {
                    IsSuccess = true,
                    Message = "Category was added successfully"
                };
            }
            catch(SqliteException ex)
            {
                if (ex.SqliteErrorCode == 19)
                    return new ResponseMsg<CategoryData>
                    {
                        IsSuccess = false,
                        Message = $"Category '{data.CategoryName}' already exists"
                    };

                return new ResponseMsg<CategoryData>
                {
                    IsSuccess = false,
                    Message = $"Database error: {ex.Message}"
                };
            }
            catch(Exception ex)
            {
                return new ResponseMsg<CategoryData>
                {
                    IsSuccess = false,
                    Message = $"Unexpected error: {ex.Message}"
                };
            }
        }

        public ResponseMsg<CategoryData> Delete(int id)
        {
            try
            {
                using var db = _context.CreateConnection();

                using var command = new SqliteCommand(DeleteSql, db);
                command.Parameters.AddWithValue(
                    DbConstants.Param(DbConstants.CategoryFields.CategoryDataId), id
                );

                var reader = command.ExecuteNonQuery();

                if(reader == 0)
                {
                    return new ResponseMsg<CategoryData>
                    {
                        IsSuccess = false,
                        Message = $"There is no category with id = {id}"
                    };
                }

                return new ResponseMsg<CategoryData>
                {
                    IsSuccess = true,
                    Message = "Category Was deleted successfully"
                };
            }
            catch(SqliteException ex)
            {
                return new ResponseMsg<CategoryData>
                {
                    IsSuccess = false,
                    Message = $"Database error: {ex.Message}"
                };
            }
            catch(Exception ex)
            {
                return new ResponseMsg<CategoryData>
                {
                    IsSuccess = false,
                    Message = $"Unexpected error: {ex.Message}"
                };
            }
        }

        public ResponseMsg<List<CategoryData>> GetAll()
        {
            List<CategoryData> data = [];

            try
            {
                using var db = _context.CreateConnection();

                using var command = new SqliteCommand(GetAllSql, db);

                using var reader = command.ExecuteReader();

                int categoryDataIdOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId));
                int categoryNameOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName));
                int iconOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.CategoryFields.Icon));

                while(reader.Read())
                {
                    CategoryData d = new()
                    {
                        CategoryDataId = reader.GetInt32(categoryDataIdOrdinal),
                        CategoryName = reader.GetString(categoryNameOrdinal),
                        Icon = reader.IsDBNull(iconOrdinal) ? null : reader.GetString(iconOrdinal)
                    };

                    data.Add(d);
                }

                return new ResponseMsg<List<CategoryData>>
                {
                    IsSuccess = true,
                    Message = "Data was received",
                    Data = data
                };
            }
            catch(SqliteException ex)
            {
                return new ResponseMsg<List<CategoryData>>
                {
                    IsSuccess = false,
                    Message = $"Database error: {ex.Message}"
                };
            }
            catch(Exception ex)
            {
                return new ResponseMsg<List<CategoryData>>
                {
                    IsSuccess = false,
                    Message = $"Unexpected error: {ex.Message}"
                };
            }
        }

        public ResponseMsg<CategoryData> GetById(int id)
        {
            try
            {
                using var db = _context.CreateConnection();

                var command = db.CreateCommand();
                command.CommandText = GetByIdSql;
                command.Parameters.AddWithValue($"{DbConstants.Param(DbConstants.CategoryFields.CategoryDataId)}", id);
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int categoryDataIdOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId));
                    int categoryNameOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName));
                    int categoryIconOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.CategoryFields.Icon));

                    CategoryData data = new()
                    {
                        CategoryDataId = id,
                        CategoryName = reader.GetString(categoryNameOrdinal),
                        Icon = reader.GetString(categoryIconOrdinal)
                    };

                    return new ResponseMsg<CategoryData>
                    {
                        IsSuccess = true,
                        Message = "Data Was found",
                        Data = data
                    };
                } 

                return new ResponseMsg<CategoryData>
                {
                    IsSuccess = false,
                    Message = "There is no such category"
                };
            }
            catch(SqliteException ex)
            {
                return new ResponseMsg<CategoryData> 
                {
                    IsSuccess = false,
                    Message = $"Db Error: {ex.Message}"
                };
            }
        }

        public ResponseMsg<CategoryData> GetByName(string name)
        {
            try
            {
                var db = _context.CreateConnection();
                
                using var command = new SqliteCommand(GetByNameSql, db);
                command.Parameters.AddWithValue(
                    DbConstants.Param(DbConstants.CategoryFields.CategoryName), name
                );

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int categoryDataIdOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId));
                    int categoryNameOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName));
                    int categoryIconOrdinal = reader.GetOrdinal(DbConstants.GetFieldName(DbConstants.CategoryFields.Icon));

                    CategoryData data = new()
                    {
                        CategoryDataId = reader.GetInt32(categoryDataIdOrdinal),
                        CategoryName = reader.GetString(categoryNameOrdinal),
                        Icon = reader.GetString(categoryIconOrdinal)
                    };

                    return new ResponseMsg<CategoryData>
                    {
                        IsSuccess = true,
                        Message = "Data Was found",
                        Data = data
                    };
                } 

                return new ResponseMsg<CategoryData>
                {
                    IsSuccess = false,
                    Message = $"There is no category with name = {name}"
                };
            }
            catch(SqliteException ex)
            {
                return new ResponseMsg<CategoryData>
                {
                    IsSuccess = false,
                    Message = $"DataBase Error: {ex.Message}"
                };
            }
            catch(Exception ex)
            {
                return new ResponseMsg<CategoryData>
                {
                    IsSuccess = false,
                    Message = $"Unexpected Error: {ex.Message}"
                };
            }
        }
        public ResponseMsg<CategoryData> Update(int id, UpdateCategoryDto updateData)
        {
            try
            {
                using var db = _context.CreateConnection();

                using var command = new SqliteCommand(UpdateSql, db);

                command.Parameters.AddWithValue(
                    DbConstants.Param(DbConstants.CategoryFields.CategoryDataId), id
                );
                command.Parameters.AddWithValue(
                    DbConstants.Param(DbConstants.CategoryFields.CategoryName), updateData.CategoryName
                );
                command.Parameters.AddWithValue(
                    DbConstants.Param(DbConstants.CategoryFields.Icon), updateData.Icon
                );

                command.ExecuteNonQuery();

                return new ResponseMsg<CategoryData>
                {
                    IsSuccess = true,
                    Message = "Data was updated successfully"
                };

            }
            catch(SqliteException ex)
            {
                return new ResponseMsg<CategoryData>
                {
                    IsSuccess = false,
                    Message = $"Database error: {ex.Message}"
                };
            }
            catch(Exception ex)
            {
                return new ResponseMsg<CategoryData>
                {
                    IsSuccess = false,
                    Message = $"Unexpected error: {ex.Message}"
                };
            }
        }
    }
}
