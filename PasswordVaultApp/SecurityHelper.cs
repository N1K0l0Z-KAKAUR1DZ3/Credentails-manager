using System.Security.Cryptography;
namespace PasswordVaultApp;

public static class SecurityHelper
{
    private const int SaltSize = 16; 
    private const int HashSize = 32; 
    private const int Iterations = 100000; 


    public static string GenerateRandomSalt()
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] salt = new byte[SaltSize];
            rng.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }     
    }

    public static string GenerateHash(string password, string salt)
    {
        byte[] rawSalt = Convert.FromBase64String(salt);
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, rawSalt, Iterations, HashAlgorithmName.SHA256))
        {
            byte[] hash = pbkdf2.GetBytes(HashSize);
            return Convert.ToBase64String(hash);
        }
    }
}