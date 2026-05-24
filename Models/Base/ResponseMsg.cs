namespace PasswordManager.Models.Base
{
    public class ResponseMsg<T>
    {
        public bool IsSuccess {get;set;}
        public string? Message {get; set;}
        public T? Data {get; set;} = default;
    }
}
