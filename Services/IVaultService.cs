using PasswordManager.Models.Base;
using PasswordManager.Models.DTO;
using PasswordManager.Models.UserData;

namespace PasswordManager.Services
{
    public interface IVaultService
    {
        public ResponseMsg AddEntry(CoreDataModel data);
        public List<CoreDataModel> GetAllEntries();
        public ResponseMsg UpdateEntry(int id, UpdateDto updateDto);
    }
}
