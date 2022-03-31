using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.RetailerManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.RetailerManagement;

internal class ValidateRetailerRegistration : IValidateRetailerRegistration
{
    private readonly IDbContext _context;

    public ValidateRetailerRegistration(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> CanRegisterAccount(AccountId identifier, CancellationToken token)
    {
        try
        {
            return Result.Success(
                await _context.Set<Retailer>().AllAsync(s => s.AccountIdentifier != identifier, token));
        }
        catch (Exception e)
        {
            return Result.Failure<bool>(ErrorKind.Unexpected, "database.error", e.Message);
        }
    }
}