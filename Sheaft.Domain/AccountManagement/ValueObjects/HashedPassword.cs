using Sheaft.Domain.AccountManagement.Services;

namespace Sheaft.Domain.AccountManagement.ValueObjects;

public record HashedPassword
{
    private HashedPassword(string hash)
    {
        Hash = hash;
    }

    public string Hash { get; }

    public static HashedPassword Create(Password password, IPasswordHasher hasher)
    {
        return Create(password.Value, hasher);
    }
    
    internal static HashedPassword Create(string password, IPasswordHasher hasher)
    {
        var hashedPassword = hasher.HashPassword(password);
        return new HashedPassword(hashedPassword);
    }
};