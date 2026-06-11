using PasswordManager.Models.UserData;

namespace PasswordManager.Models.DTO
{
    public class UpdateDto
    {
        public string? Website {get; set;}
        public string? Email {get; set;}
        public string? Password {get; set;}
        public string? Url {get; set;}
        public string? Description {get; set;}
        public int? CategoryId {get; set;}
        public DateTime LastModifiedDate {get; set;} = DateTime.UtcNow;
    }
}
