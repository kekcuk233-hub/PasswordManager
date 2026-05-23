using PasswordManager.Models.Userdata;

namespace PasswordManager.Models.UserData
{
    public class CoreDataModel
    {
        public int Id {get; set;}
        public string Website {get;set;}
        public string Email {get; set;}
        public string Password {get; set;} 
        public string? Url {get; set;}
        public string? Description {get; set;}
        //public int CategoryId {get; set;} //I need to make foreign key for link many-to-one
        public string? Category {get; set;}
        public DateTime CreationDate {get; set;} = DateTime.UtcNow;
        public DateTime LastModifiedDate {get; set;} = DateTime.UtcNow;
    }
}
