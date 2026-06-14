namespace PasswordManager.Services.Utils
{
    public interface IPasswordGeneratorService
    {
        string Generate(PasswordGeneratorService.PasswordOptions options);
        PasswordStrength CheckStrength(string password);
        bool TryGenerate(
            PasswordGeneratorService.PasswordOptions options,
            out string? password,
            out string? error
            );
    }
}
