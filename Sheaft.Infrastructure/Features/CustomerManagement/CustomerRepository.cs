using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.CustomerManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.CustomerManagement;

internal class CustomerRepository : Repository<Customer, CustomerId>, ICustomerRepository
{
    public CustomerRepository(IDbContext context)
        : base(context)
    {
    }

    public override Task<Result<Customer>> Get(CustomerId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .SingleOrDefaultAsync(e => e.Identifier == identifier, token);

            return result != null
                ? Result.Success(result)
                : Result.Failure<Customer>(ErrorKind.NotFound, "customer.not.found");
        });
    }

    public Task<Result<Customer>> Get(AccountId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .SingleOrDefaultAsync(e => e.AccountIdentifier == identifier, token);

            return result != null
                ? Result.Success(result)
                : Result.Failure<Customer>(ErrorKind.NotFound, "customer.not.found");
        });
    }

    public Task<Result<IEnumerable<CustomerInformation>>> GetInfo(IEnumerable<CustomerId> identifiers, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .Where(e => identifiers.Contains(e.Identifier))
                .Select(e => new CustomerInformation(e.Identifier, e.TradeName.Value))
                .ToListAsync(token);

            return Result.Success(result.AsEnumerable());
        });
    }
}