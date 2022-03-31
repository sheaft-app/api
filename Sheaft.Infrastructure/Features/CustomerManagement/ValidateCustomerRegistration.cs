using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.CustomerManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.CustomerManagement;

internal class ValidateCustomerRegistration : IValidateCustomerRegistration
{
    private readonly IDbContext _context;

    public ValidateCustomerRegistration(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> CanRegisterAccount(AccountId identifier, CancellationToken token)
    {
        try
        {
            return Result.Success(
                await _context.Set<Customer>().AllAsync(s => s.AccountIdentifier != identifier, token));
        }
        catch (Exception e)
        {
            return Result.Failure<bool>(ErrorKind.Unexpected, "database.error", e.Message);
        }
    }
}