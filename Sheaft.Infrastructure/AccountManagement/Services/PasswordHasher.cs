using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Sheaft.Domain.AccountManagement;

namespace Sheaft.Infrastructure.AccountManagement;

public class PasswordHasher : IPasswordHasher
{
    private readonly byte[] _salt;

    public PasswordHasher(IOptionsSnapshot<JwtSettings> jwtSettings)
    {
        _salt = Encoding.Unicode.GetBytes(jwtSettings.Value.Salt);
    }
    
    public PasswordHasher(string salt)
    {
        _salt = Encoding.Unicode.GetBytes(salt);
    }

    public (string hashPassword, string salt) CreatePassword(string password)
    {
        var specificSalt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var saltedPassword = AppendSaltToPassword(password, specificSalt);
        
        return (HashPassword(saltedPassword), specificSalt);
    }

    private static string AppendSaltToPassword(string password, string specificSalt)
    {
        return $"{password}{specificSalt}";
    }

    public bool PasswordIsValid(string password, HashedPassword hash)
    {
        var saltedPassword = AppendSaltToPassword(password, hash.Salt);
        var hashedPassword = HashPassword(saltedPassword);

        return hashedPassword == hash.Hash;
    }

    private string HashPassword(string saltedPassword)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            saltedPassword,
            _salt,
            KeyDerivationPrf.HMACSHA256,
            10000,
            50));
    }
}