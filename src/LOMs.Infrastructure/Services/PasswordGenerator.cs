using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Common.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace LOMs.Infrastructure.Services;

public class PasswordGenerator(IOptions<PasswordGeneratorOptions> passwordOptions) : IPasswordGenerator
{
    private readonly byte[] _key = Encoding.UTF8.GetBytes(passwordOptions.Value.Key.PadRight(32).Substring(0, 32));
    private readonly string _tt = "Temp-"; // Temp Token


    public string GenerateTempPassword()
    {
        // Step 1: Generate an 8-character random password
        string tempPassword = _tt + GenerateRandom(8); // e.g., "Temp-ab12cd34"

        // Step 2: Encrypt the temporary password
        return Encrypt(tempPassword);
    }

    public string? IsTempPassword(string password)
    {
        var decryptedPassword = Decrypt(password);

        return decryptedPassword.StartsWith(_tt) ? decryptedPassword : null;
    }

    public string EncryptPassword(string password)
    {
        // Step 1: Prepend "Temp-" if not already present (for manual encryption calls)
        string passwordToEncrypt = password.StartsWith("Temp-") ? password : "Temp-" + password;

        // Step 2: Encrypt the password
        return Encrypt(passwordToEncrypt);
    }

    public string DecryptPassword(string encryptedPassword)
    {
        // Step 1: Decrypt the password
        return Decrypt(encryptedPassword);
    }

    public string GenerateRandom(int num)
    {
        return Guid.NewGuid().ToString("N")[..num]; // e.g., "Temp-ab12cd34"
    }

    private string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.GenerateIV();
        var iv = aes.IV;

        using var encryptor = aes.CreateEncryptor(aes.Key, iv);
        using var ms = new MemoryStream();
        ms.Write(iv, 0, iv.Length); // Prepend IV
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }
        return Convert.ToBase64String(ms.ToArray());
    }

    private string Decrypt(string cipherText)
    {
        var fullCipher = Convert.FromBase64String(cipherText);
        using var aes = Aes.Create();
        aes.Key = _key;
        var iv = new byte[16];
        Array.Copy(fullCipher, 0, iv, 0, iv.Length);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }

}