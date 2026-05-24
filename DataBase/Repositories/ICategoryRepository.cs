using PasswordManager.Models.Base;
using PasswordManager.Models.UserData;

namespace PasswordManager.DataBase.Repositories
{
    public interface ICategoryRepository
    {
        List<CategoryData> GetAll();
        ResponseMsg<CategoryData> Add();
        ResponseMsg<CategoryData> Update();
        ResponseMsg<CategoryData> Get(int id);
        ResponseMsg<CategoryData> Delete();
    }
}
