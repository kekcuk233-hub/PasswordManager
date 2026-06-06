using PasswordManager.Models.Base;
using PasswordManager.Models.DTO;
using PasswordManager.Models.UserData;

namespace PasswordManager.Services
{
    public interface IVaultService
    {
        ResponseMsg AddEntry(CoreDataModel data);
        ResponseMsg<CoreDataModel> GetEntryById(int id);
        ResponseMsg<List<CoreDataModel>> GetAllEntries();
        ResponseMsg<CoreDataModel> UpdateEntry(int id, UpdateDto updateDto);
        ResponseMsg DeleteEntry(int id);
        ResponseMsg<List<CategoryData>> GetAllCategory();
        ResponseMsg<CategoryData> GetCategoryById(int id);
        ResponseMsg AddCategory(CategoryData model);
        ResponseMsg<CategoryData> UpdateCategory(int id, UpdateCategoryDto dto);
        ResponseMsg DeleteCategory(int id);
    }
}
