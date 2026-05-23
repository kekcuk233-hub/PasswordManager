using PasswordManager.Models.Base;
using PasswordManager.Models.DTO;
using PasswordManager.Models.UserData;

namespace PasswordManager.DataBase.Repositories
{
    public interface IEntryRepository
    {
        List<CoreDataModel> GetAll();
        ResponseMsg Insert(CoreDataModel data);
        ResponseMsg Update(int id, UpdateDto data);
        ResponseMsg Delete(int id);
    }
}
