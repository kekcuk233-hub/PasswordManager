namespace PasswordManager.DataBase
{
    public static class DbConstants
    {
        public const string PasswordTable = "Passwords";
        public const string CategoryTable = "Categories";
        public const string DefaultCategoryName = "Unassigned";

        public enum PasswordFields
        {
            PasswordId,
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
            CategoryDataId,
            CategoryName,
            Icon
        }

        public static string GetFieldName(PasswordFields field) => field.ToString();
        public static string GetFieldName(CategoryFields field) => field.ToString();

        public static string Param(PasswordFields field) => $"@{field}";
        public static string Param(CategoryFields field) => $"@{field}";
    }
}
