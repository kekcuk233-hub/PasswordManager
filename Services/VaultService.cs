using PasswordManager.DataBase;
using PasswordManager.DataBase.Repositories;
using PasswordManager.Models.Base;
using PasswordManager.Models.DTO;
using PasswordManager.Models.UserData;
using PasswordManager.Services.Utils;

namespace PasswordManager.Services
{
    public class VaultService(IEntryRepository entry,
                        ICategoryRepository category,
                        ICryptoService cryptoService,
                        UserSession session) : IVaultService
    {
        
        private readonly IEntryRepository _entryRepo = entry; 
        private readonly ICategoryRepository _categoryRepo = category;
        private readonly ICryptoService _cryproService = cryptoService;
        private readonly UserSession _session = session;

        public ResponseMsg AddEntry(CoreDataModel data)
        {
            if(string.IsNullOrWhiteSpace(data.Website))
            {
                return new ResponseMsg
                {
                    IsSuccess = false,
                    Message = "Please, provide Website name",
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

            if(!data.CategoryId.HasValue)
            {
                var categoryResponse = _categoryRepo.GetById(1);
                if(categoryResponse.IsSuccess && categoryResponse.Data != null)
                {
                    data.CategoryId = categoryResponse.Data.CategoryDataId;
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
                #nullable disable
                updateDto.Website = string.IsNullOrWhiteSpace(updateDto.Website) ? currentData.Website : updateDto.Website;
                updateDto.Email = string.IsNullOrWhiteSpace(updateDto.Email) ? currentData.Email : updateDto.Email;
                updateDto.Password = string.IsNullOrWhiteSpace(updateDto.Password) ? currentData.Password : updateDto.Password;
                updateDto.Url = string.IsNullOrWhiteSpace(updateDto.Url) ? currentData.Url : updateDto.Url;
                updateDto.Description = string.IsNullOrWhiteSpace(updateDto.Description) ? currentData.Description : updateDto.Description;
                updateDto.CategoryId ??= currentData.CategoryId;
                updateDto.LastModifiedDate = DateTime.UtcNow;

            return _entryRepo.Update(id, updateDto);
        }

        public ResponseMsg DeleteEntry(int id)
        {
            var defaultCategory = _categoryRepo.GetByName(DbConstants.DefaultCategoryName);
            if (defaultCategory.Data.CategoryDataId == id)
            {
                return new ResponseMsg
                {
                    IsSuccess = false,
                    Message = "Cannot delete default Category"
                };
            }

            var reassign = _entryRepo.ReassignCategory(id, defaultCategory.Data.CategoryDataId);
            
            if(!reassign.IsSuccess)
            {
                return reassign;
            }

            return _entryRepo.Delete(id);
        }

        public ResponseMsg<List<CategoryData>> GetAllCategory()
        {
            return _categoryRepo.GetAll();
        }

        public ResponseMsg<CategoryData> GetCategoryById(int id)
        {   
            return _categoryRepo.GetById(id);
        }

        public ResponseMsg AddCategory(CategoryData model)
        {
            if(string.IsNullOrWhiteSpace(model.CategoryName))
            {
                return ResponseMsg.Failure("Please, write a name for category");
            }

            return _categoryRepo.Add(model);
        }

        public ResponseMsg<CategoryData> UpdateCategory(int id, UpdateCategoryDto dto)
        {
            if(string.IsNullOrWhiteSpace(dto.CategoryName))
            {
                return new ResponseMsg<CategoryData>
                {
                    IsSuccess = false,
                    Message = "Please, write a name for category"
                };
            }

            return _categoryRepo.Update(id, dto);
        }

        public ResponseMsg DeleteCategory(int id)
        {
            return _categoryRepo.Delete(id);
        }
    }
}
