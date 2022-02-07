namespace Sheaft.Domain.AccountManagement.Services;

public interface IPasswordHasher
{
    string HashPassword(string password);
}