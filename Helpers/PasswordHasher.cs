using System.Security.Cryptography;
using System.Text;

namespace SalonBooking.Api.Helpers;

public static class PasswordHasher
{
    public static string Hash(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }

    public static bool Verify(string plainText, string hash)
    {
        return Hash(plainText) == hash;
    }
}