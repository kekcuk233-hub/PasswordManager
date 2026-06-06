
namespace PasswordManager.Models.UserData
{
    public class CategoryData
    {
        public int CategoryDataId {get; set;}
        public string CategoryName {get; set;} = string.Empty;
        public string? Icon {get; set;}
        public List<CoreDataModel> Entries {get; set;} = [];
    }
}
