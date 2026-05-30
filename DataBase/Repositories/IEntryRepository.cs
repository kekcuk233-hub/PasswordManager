using PasswordManager.Models.Base;
using PasswordManager.Models.DTO;
using PasswordManager.Models.UserData;

namespace PasswordManager.DataBase.Repositories
{
    public interface IEntryRepository
    {
        ResponseMsg<List<CoreDataModel>> GetAll();
        ResponseMsg<CoreDataModel> GetById(int id);
        ResponseMsg<CoreDataModel> Insert(CoreDataModel data);
        ResponseMsg<CoreDataModel> Update(int id, UpdateDto data);
        ResponseMsg<CoreDataModel> Delete(int id);
        ResponseMsg<CoreDataModel> ReassignCategory(int id, int defaultId);
    }
}
