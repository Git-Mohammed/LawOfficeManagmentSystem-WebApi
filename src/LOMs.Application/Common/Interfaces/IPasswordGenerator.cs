namespace LOMs.Application.Common.Interfaces;

public interface IPasswordGenerator
{
    string GenerateTempPassword();
    string? IsTempPassword(string password);
    string DecryptPassword(string password);
    string EncryptPassword(string password);
}