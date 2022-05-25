using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.CustomerManagement;
using Sheaft.Domain.SupplierManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.AccountManagement;

public class RetrieveProfile : IRetrieveProfile
{
    private readonly IDbContext _context;

    public RetrieveProfile(IDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<Maybe<Profile>>> GetAccountProfile(AccountId identifier, CancellationToken token)
    {
        try
        {
            var supplier = await _context.Set<Supplier>()
                .SingleOrDefaultAsync(s => s.AccountIdentifier == identifier, token);

            if (supplier != null)
                return Result.Success(
                    Maybe<Profile>.From(new Profile(supplier.Identifier.Value, supplier.TradeName.Value,
                        ProfileKind.Supplier)));

            var customer = await _context.Set<Customer>()
                .SingleOrDefaultAsync(s => s.AccountIdentifier == identifier, token);

            return Result.Success(customer != null ? 
                Maybe<Profile>.From(new Profile(customer.Identifier.Value, customer.TradeName.Value, ProfileKind.Customer)) 
                : Maybe<Profile>.None);

        }
        catch (Exception e)
        {
            return Result.Failure<Maybe<Profile>>(ErrorKind.Unexpected, "database.error", e.Message);
        }
    }
}