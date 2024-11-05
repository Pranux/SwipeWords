using System.Security.Cryptography;
using System.Text;

namespace SwipeWords.Models;

public static class PasswordHasher
{
    public static string GenerateSalt()
    {
        var saltBytes = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }

    public static string HashPassword(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = salt + password;
            var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
            var hashBytes = sha256.ComputeHash(saltedPasswordBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }

    public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
    {
        var hashOfEnteredPassword = HashPassword(enteredPassword, storedSalt);
        return hashOfEnteredPassword == storedHash;
    }
}