using Microsoft.Data.Sqlite;
using PasswordManager.Models.Base;
using PasswordManager.Models.UserData;

namespace PasswordManager.DataBase.Repositories
{
    public class CategoryRepository(DataBaseContext context) : ICategoryRepository
    {
        private readonly DataBaseContext _context = context;
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
            string field = DbConstants.GetFieldName(DbConstants.CategoryFields.Id); 
            string findById = $@"select * from {DbConstants.CategoryTable} 
            where {field} = @{field}";

            try
            {
                using var db = _context.CreateConnection();
                db.Open();

                var command = new SqliteCommand(findById, db);
                command.Parameters.AddWithValue($"@{field}", id);
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    CategoryData data = new()
                    {
                        Id = id,
                        CategoryName = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Icon = reader.IsDBNull(2) ? null : reader.GetString(2)
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
