using PasswordManager.Models.Base;
using PasswordManager.Models.UserData;
using PasswordManager.Services.Utils;
using PasswordManager.UI;


namespace PasswordManagerUI.PasswordManagerUI.UI;

public class UnoDisplayHandler : IDisplayHandler
{
    public void ShowCategories(List<CategoryData> categories)
    {
        throw new NotImplementedException();
    }

    public void ShowEntries(List<CoreDataModel> entries)
    {
        throw new NotImplementedException();
    }

    public void ShowPassword(string password)
    {
        throw new NotImplementedException();
    }

    public void ShowPasswordStrength(PasswordStrength strength)
    {
        throw new NotImplementedException();
    }

    public void ShowResult(ResponseMsg result)
    {
        throw new NotImplementedException();
    }

    public void ShowResult<T>(ResponseMsg<T> result) where T : class
    {
        throw new NotImplementedException();
    }
}
