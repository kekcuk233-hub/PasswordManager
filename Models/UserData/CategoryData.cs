
namespace PasswordManager.Models.UserData
{
    public class CategoryData
    {
        public int Id {get; set;}
        public string? CategoryName {get; set;}
        public string? Icon {get; set;}
        public List<CoreDataModel> Entries {get; set;} = [];
    }
}
