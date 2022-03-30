using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.AccountManagement;

internal class UniquenessValidator : IUniquenessValidator
{
    private readonly IDbContext _context;

    public UniquenessValidator(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> UsernameAlreadyExists(Username username, CancellationToken token)
    {
        try
        {
            var exists = await _context.Set<Account>().AnyAsync(a => a.Username == username, token);
            return Result.Success(exists);
        }
        catch (Exception e)
        {
            return Result.Failure<bool>(ErrorKind.Unexpected, "database.error", e.Message);
        }
    }

    public async Task<Result<bool>> EmailAlreadyExists(EmailAddress email, CancellationToken token)
    {
        try
        {
            var exists = await _context.Set<Account>().AnyAsync(a => a.Email == email, token);
            return Result.Success(exists);
        }
        catch (Exception e)
        {
            return Result.Failure<bool>(ErrorKind.Unexpected, "database.error", e.Message);
        }
    }
}