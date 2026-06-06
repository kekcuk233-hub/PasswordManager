using PasswordManager.Models.Base;
using PasswordManager.Models.DTO;
using PasswordManager.Models.UserData;

namespace PasswordManager.DataBase.Repositories
{
    public interface ICategoryRepository
    {
        ResponseMsg<List<CategoryData>> GetAll();
        ResponseMsg Add(CategoryData data);
        ResponseMsg<CategoryData> Update(int id, UpdateCategoryDto data);
        ResponseMsg<CategoryData> GetById(int id);
        ResponseMsg<CategoryData> GetByName(string name);
        ResponseMsg Delete(int id);
    }
}
