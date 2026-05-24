namespace PasswordManager.DataBase
{
    public static class DbConstants
    {
        public const string PasswordTable = "Passwords";
        public const string CategoryTable = "Categories";

        public enum PasswordFields
        {
            Id,
            Website,
            Email,
            Password,
            Url,
            Description,
            CategoryId,
            CreationDate,
            LastModifiedDate
        }

        public enum CategoryFields
        {
            Id,
            CategoryName,
            Icon
        }

        public static string GetFieldName(PasswordFields field)
        {
            return field.ToString();
        }

        public static string GetFieldName(CategoryFields field)
        {
            return field.ToString();
        }
    }
}
