using PasswordManager.Models.Base;
using PasswordManager.Models.DTO;
using PasswordManager.Models.UserData;

namespace PasswordManager.DataBase.Repositories
{
    public interface IEntryRepository
    {
        ResponseMsg<List<CoreDataModel>> GetAll();
        ResponseMsg<CoreDataModel> GetById(int id);
        ResponseMsg Insert(CoreDataModel data);
        ResponseMsg<CoreDataModel> Update(int id, UpdateDto data);
        ResponseMsg Delete(int id);
        ResponseMsg<List<CoreDataModel>> GetByCategoryId(int id);
        ResponseMsg ReassignCategory(int id, int defaultId);
    }
}
