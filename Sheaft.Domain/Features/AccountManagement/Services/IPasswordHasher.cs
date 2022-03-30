namespace Sheaft.Domain.AccountManagement;

public interface IPasswordHasher
{
    (string hashPassword, string salt) CreatePassword(string password);
    bool PasswordIsValid(string password, HashedPassword hash);
}