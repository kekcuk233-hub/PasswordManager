using PasswordManager.Models.Base;
using PasswordManager.Models.UserData;
using PasswordManager.Services.Utils;

namespace PasswordManager.UI
{
    public interface IDisplayHandler
    {
        void ShowResult(ResponseMsg result);
        void ShowResult<T>(ResponseMsg<T> result) where T : class;
        void ShowEntries(List<CoreDataModel> entries);
        void ShowCategories(List<CategoryData> categories);
        void ShowPassword(string password);
        void ShowPasswordStrength(PasswordStrength strength);
    }
}
