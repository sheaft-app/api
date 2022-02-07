using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Sheaft.Domain.AccountManagement.Services;

namespace Sheaft.Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    private readonly byte[] _salt;

    public PasswordHasher()
    {
        _salt = RandomNumberGenerator.GetBytes(128/8);
    }

    public string HashPassword(string password)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password,
            _salt,
            KeyDerivationPrf.HMACSHA256,
            10000,
            256 / 8));
    }
}