using PasswordManager.Models.Base;
using PasswordManager.Models.DTO;
using PasswordManager.Models.UserData;

namespace PasswordManager.DataBase.Repositories
{
    public interface ICategoryRepository
    {
        ResponseMsg<List<CategoryData>> GetAll();
        ResponseMsg<CategoryData> Add(CategoryData data);
        ResponseMsg<CategoryData> Update(int id, UpdateCategoryDto data);
        ResponseMsg<CategoryData> Get(int id);
        ResponseMsg<CategoryData> Delete(int id);
    }
}
