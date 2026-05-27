using Microsoft.Data.Sqlite;
using PasswordManager.Models.Base;
using PasswordManager.Models.UserData;

namespace PasswordManager.DataBase.Repositories
{
    public class CategoryRepository(DataBaseContext context) : ICategoryRepository
    {
        private readonly DataBaseContext _context = context;

        private static readonly string GetSql = $@"
        select
        {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId)},
        {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryName)},
        {DbConstants.GetFieldName(DbConstants.CategoryFields.Icon)}
        from {DbConstants.CategoryTable} 
        where {DbConstants.GetFieldName(DbConstants.CategoryFields.CategoryDataId)} = {DbConstants.Param(DbConstants.CategoryFields.CategoryDataId)}";
        public ResponseMsg<CategoryData> Add()
        {
            throw new NotImplementedException();
        }

        public ResponseMsg<CategoryData> Delete()
        {
            throw new NotImplementedException();
        }

        public List<CategoryData> GetAll()
        {
            throw new NotImplementedException();
        }

        public ResponseMsg<CategoryData> Get(int id)
        {
            try
            {
                using var db = _context.CreateConnection();

                var command = db.CreateCommand();
                command.CommandText = GetSql;
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

        public ResponseMsg<CategoryData> Update()
        {
            throw new NotImplementedException();
        }
    }
}
