namespace PasswordManager.Models.Base
{
    public class ResponseMsg<T> where T : class
    {
        public bool IsSuccess {get;set;}
        public string? Message {get; set;}
        public T? Data {get; set;} = default;
    }

    public class ResponseMsg
    {
        public bool IsSuccess {get;set;}
        public string? Message {get; set;}
        
        public static ResponseMsg Success(string message) => new() {IsSuccess = true, Message = message};
        public static ResponseMsg Failure(string message) => new() {IsSuccess = false, Message = message};
    }
}
