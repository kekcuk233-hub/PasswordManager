using PasswordManager.Models.DTO;
using PasswordManager.Models.UserData;
using PasswordManager.Services.Utils;

namespace PasswordManager.UI
{
    public interface IMenuHandler
    {
        AuthMenuOption   PromptAuthMenu();
        MainMenuOption   PromptMainMenu();
        CategoryMenuOption PromptCategoryMenu();
        string           PromptMasterPassword(string prompt);
        int              PromptId(string prompt);
        string           PromptKeyword();
        CoreDataModel    PromptNewEntry(List<CategoryData> categories);
        UpdateDto        PromptUpdateEntry(List<CategoryData> categories);
        CategoryData     PromptNewCategory();
        PasswordGeneratorService.PasswordOptions PromptPasswordOptions();
    }
}
