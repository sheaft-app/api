using Sheaft.Domain.AccountManagement;

namespace Sheaft.Domain;

public record HashedPassword
{
    private HashedPassword(string hash, string salt)
    {
        Hash = hash;
        Salt = salt;
    }

    public string Hash { get; private set; }
    public string Salt { get; private set; }

    public static HashedPassword Create(Password password, IPasswordHasher hasher)
    {
        return Create(password.Value, hasher);
    }

    internal static HashedPassword FromHashedString(string hashedPassword, string salt)
    {
        return new HashedPassword(hashedPassword, salt);
    }
    
    public static HashedPassword Create(string password, IPasswordHasher hasher)
    {
        (string hashedPassword, string usedSalt) = hasher.CreatePassword(password);
        return new HashedPassword(hashedPassword, usedSalt);
    }
}