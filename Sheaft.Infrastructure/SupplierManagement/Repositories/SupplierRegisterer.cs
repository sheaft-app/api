using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.SupplierManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.SupplierManagement;

internal class SupplierRegisterer : ISupplierRegisterer
{
    private readonly IDbContext _context;

    public SupplierRegisterer(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> CanRegisterAccountAsSupplier(AccountId identifier, CancellationToken token)
    {
        try
        {
            return Result.Success(
                await _context.Set<Supplier>().AllAsync(s => s.AccountIdentifier != identifier, token));
        }
        catch (Exception e)
        {
            return Result.Failure<bool>(ErrorKind.Unexpected, "database.error", e.Message);
        }
    }
}