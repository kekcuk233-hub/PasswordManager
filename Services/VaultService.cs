using PasswordManager.DataBase.Repositories;
using PasswordManager.Models.Base;
using PasswordManager.Models.DTO;
using PasswordManager.Models.UserData;

namespace PasswordManager.Services
{
    public class VaultService : IVaultService
    {
        private readonly IEntryRepository _entryRepo; 
        private readonly ICategoryRepository _categoryRepo;

        public VaultService(IEntryRepository entry, ICategoryRepository category)
        {
            _entryRepo = entry;
            _categoryRepo = category;
        }

        public ResponseMsg<CoreDataModel> AddEntry(CoreDataModel data)
        {
            if(string.IsNullOrWhiteSpace(data.Website))
            {
                return new ResponseMsg<CoreDataModel>
                {
                    IsSuccess = false,
                    Message = "Please, provide Website name",
                    Data = null
                };
            }

            if(string.IsNullOrWhiteSpace(data.Email))
            {
                return new ResponseMsg<CoreDataModel>
                {
                    IsSuccess = false,
                    Message = "Please, provide Email/Username"
                };
            }

            if(string.IsNullOrWhiteSpace(data.Password))
            {
                return new ResponseMsg<CoreDataModel>
                {
                    IsSuccess = false,
                    Message = "Please, provide a strong password or generate it"
                };
            }

            if(!data.CategoryId.HasValue)
            {
                var categoryResponse = _categoryRepo.Get(1);
                if(categoryResponse.IsSuccess && categoryResponse.Data != null)
                {
                    data.CategoryId = categoryResponse.Data.Id;
                }
            }

            if(string.IsNullOrWhiteSpace(data.Url))
            {
                data.Url = null;
            }

            if(string.IsNullOrWhiteSpace(data.Description))
            {
                data.Description = null;
            }

            return _entryRepo.Insert(data);
        }

        public ResponseMsg<CoreDataModel> GetEntryById(int id)
        {
            return _entryRepo.GetById(id);
        }

        public ResponseMsg<List<CoreDataModel>> GetAllEntries()
        {
            return _entryRepo.GetAll();
        }

        public ResponseMsg<CoreDataModel> UpdateEntry(int id, UpdateDto updateDto)
        {
            var currentDataResponse = GetEntryById(id);

                if(!currentDataResponse.IsSuccess)
                {
                    return currentDataResponse;
                }

                var currentData = currentDataResponse.Data;

                updateDto.Website = string.IsNullOrWhiteSpace(updateDto.Website) ? currentData.Website : updateDto.Website;
                updateDto.Email = string.IsNullOrWhiteSpace(updateDto.Email) ? currentData.Email : updateDto.Email;
                updateDto.Password = string.IsNullOrWhiteSpace(updateDto.Password) ? currentData.Password : updateDto.Password;
                updateDto.Url = string.IsNullOrWhiteSpace(updateDto.Url) ? currentData.Url : updateDto.Url;
                updateDto.Description = string.IsNullOrWhiteSpace(updateDto.Description) ? currentData.Description : updateDto.Description;
                updateDto.CategoryId = updateDto.CategoryId ?? currentData.CategoryId;
                updateDto.LastModifiedDate = DateTime.UtcNow;

            return _entryRepo.Update(id, updateDto);
        }

        public ResponseMsg<CoreDataModel> DeleteEntry(int id)
        {
            return _entryRepo.Delete(id);
        }
    }
}
