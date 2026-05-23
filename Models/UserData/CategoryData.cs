using PasswordManager.Models.UserData;

namespace PasswordManager.Models.Userdata
{
    public class CategoryData
    {
        public int Id {get; set;}
        public required string Category {get; set;}
        public List<CoreDataModel> Models {get; set;} = [];
    }
}
