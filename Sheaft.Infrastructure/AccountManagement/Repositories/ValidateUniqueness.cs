using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.AccountManagement;

internal class ValidateUniqueness : IValidateUniqueness
{
    private readonly IDbContext _context;

    public ValidateUniqueness(IDbContext context)
    {
        _context = context;
    }

    public virtual async Task<Result<bool>> IsUsernameAlreadyExists(Username username, CancellationToken token)
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

    public virtual async Task<Result<bool>> IsEmailAlreadyExists(EmailAddress email, CancellationToken token)
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