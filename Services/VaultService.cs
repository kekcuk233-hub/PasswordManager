using PasswordManager.DataBase.Repositories;
using PasswordManager.Models.Base;
using PasswordManager.Models.DTO;
using PasswordManager.Models.UserData;

namespace PasswordManager.Services
{
    public class VaultService : IVaultService
    {
        private readonly IEntryRepository _entryRepo; 

        public VaultService(IEntryRepository entry)
        {
            _entryRepo = entry;
        }

        public ResponseMsg AddEntry(CoreDataModel data)
        {
            if(string.IsNullOrWhiteSpace(data.Website))
            {
                return new ResponseMsg
                {
                    IsSuccess = false,
                    Message = "Please, provide Website name"
                };
            }

            if(string.IsNullOrWhiteSpace(data.Email))
            {
                return new ResponseMsg
                {
                    IsSuccess = false,
                    Message = "Please, provide Email/Username"
                };
            }

            if(string.IsNullOrWhiteSpace(data.Password))
            {
                return new ResponseMsg
                {
                    IsSuccess = false,
                    Message = "Please, provide a strong password or generate it"
                };
            }

            return _entryRepo.Insert(data);
        }

        public List<CoreDataModel> GetAllEntries()
        {
            return _entryRepo.GetAll();
        }

        public ResponseMsg UpdateEntry(int id, UpdateDto updateDto)
        {
            if(string.IsNullOrWhiteSpace(updateDto.Website))
            {
                return new ResponseMsg
                {
                    IsSuccess = false,
                    Message = "Please, provide Website name"
                };
            }

            if(string.IsNullOrWhiteSpace(updateDto.Email))
            {
                return new ResponseMsg
                {
                    IsSuccess = false,
                    Message = "Please, provide Email/Username"
                };
            }

            if(string.IsNullOrWhiteSpace(updateDto.Password))
            {
                return new ResponseMsg
                {
                    IsSuccess = false,
                    Message = "Please, provide a strong password or generate it"
                };
            }

            return _entryRepo.Update(id, updateDto);
        }
    }
}
