namespace PasswordManager.DataBase
{
    public static class DbConstants
    {
        public const string PasswordTable = "Passwords";

        public enum PasswordFields
        {
            Website,
            Email,
            Password,
            Url,
            Description,
            Category,
            CreationDate,
            LastModifiedDate
        }

        public static string GetFieldName(PasswordFields field)
        {
            return field.ToString();
        }
    }
}
