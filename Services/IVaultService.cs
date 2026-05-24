using PasswordManager.Models.Base;
using PasswordManager.Models.DTO;
using PasswordManager.Models.UserData;

namespace PasswordManager.Services
{
    public interface IVaultService
    {
        public ResponseMsg<CoreDataModel> AddEntry(CoreDataModel data);
        public ResponseMsg<List<CoreDataModel>> GetAllEntries();
        public ResponseMsg<CoreDataModel> UpdateEntry(int id, UpdateDto updateDto);
        public ResponseMsg<CoreDataModel> DeleteEntry(int id);
    }
}
