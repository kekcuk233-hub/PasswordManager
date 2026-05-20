using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Models
{
    public class UserData
    {
        [Required]
        public required int Id {get; set;}
        public required string Website {get;set;}
        [Required]
        public required string Password {get; set;} 
        public string? Description {get; set;}
    }
}
