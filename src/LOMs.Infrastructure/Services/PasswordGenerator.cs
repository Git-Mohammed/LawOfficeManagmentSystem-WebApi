using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Common.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace LOMs.Infrastructure.Services;

/// <summary>
/// Provides secure password generation and hashing utilities.
/// </summary>
public class PasswordGenerator(IOptions<PasswordGeneratorOptions> passwordOptions) : IPasswordGenerator
{
    // Constants for PBKDF2
    private const int SaltSize = 16;          // 128-bit salt
    private const int KeySize = 32;           // 256-bit key
    private const int Iterations = 100_000;   // PBKDF2 iterations
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

    // Prefix for temporary passwords (hashed before use)
    private const string TempPrefix = "Temp-";

    /// <summary>
    /// Generates a temporary password with a hashed prefix and strong random part.
    /// </summary>
    public string GenerateTempPassword()
    {
        string prefixHash = HashPrefix(TempPrefix);
        string randomPart = GenerateComplexPassword(12);
        return prefixHash + randomPart;
    }

    /// <summary>
    /// Hashes a password using PBKDF2 with SHA256.
    /// </summary>
    public string HashPassword(string password)
    {
        using var deriveBytes = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithm);
        var salt = deriveBytes.Salt;
        var key = deriveBytes.GetBytes(KeySize);

        var result = new byte[salt.Length + key.Length];
        Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
        Buffer.BlockCopy(key, 0, result, salt.Length, key.Length);

        return Convert.ToBase64String(result);
    }

    /// <summary>
    /// Verifies a password against a stored PBKDF2 hash.
    /// </summary>
    public bool VerifyPassword(string password, string storedHash)
    {
        var fullBytes = Convert.FromBase64String(storedHash);
        var salt = new byte[SaltSize];
        var key = new byte[KeySize];

        Buffer.BlockCopy(fullBytes, 0, salt, 0, salt.Length);
        Buffer.BlockCopy(fullBytes, salt.Length, key, 0, key.Length);

        using var deriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithm);
        var newKey = deriveBytes.GetBytes(KeySize);

        return key.SequenceEqual(newKey);
    }

    /// <summary>
    /// Generates a strong password that satisfies Identity rules:
    /// - At least one uppercase, lowercase, digit, and symbol.
    /// - Minimum length enforced.
    /// </summary>
    private string GenerateComplexPassword(int length)
    {
        if (length < 8) length = 8; // enforce minimum

        const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lower = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string symbols = "!@#$%^&*()-_=+[]{}<>?";

        string allChars = upper + lower + digits + symbols;
        var chars = new List<char>
        {
            GetRandomChar(upper),
            GetRandomChar(lower),
            GetRandomChar(digits),
            GetRandomChar(symbols)
        };

        while (chars.Count < length)
            chars.Add(GetRandomChar(allChars));

        // Shuffle to avoid predictable order
        return new string(chars.OrderBy(_ => Guid.NewGuid()).ToArray());
    }

    /// <summary>
    /// Returns a random character from the given charset using a secure RNG.
    /// </summary>
    private static char GetRandomChar(string charset)
    {
        var data = RandomNumberGenerator.GetBytes(4);
        int value = BitConverter.ToInt32(data, 0) & int.MaxValue;
        return charset[value % charset.Length];
    }

    /// <summary>
    /// Hashes the prefix using SHA256 and returns a short fragment.
    /// </summary>
    private static string HashPrefix(string prefix)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(prefix);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash)[..6]; // short, non-obvious prefix
    }
}
