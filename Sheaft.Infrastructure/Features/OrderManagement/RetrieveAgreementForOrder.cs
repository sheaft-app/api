using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.AgreementManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.OrderManagement;

public class RetrieveAgreementForOrder : IRetrieveAgreementForOrder
{
    private readonly IDbContext _context;

    public RetrieveAgreementForOrder(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> IsExistingBetweenSupplierAndCustomer(SupplierId supplierIdentifier, CustomerId customerIdentifier,
        CancellationToken token)
    {
        try
        {
            var agreement = await _context.Set<Agreement>()
                .SingleOrDefaultAsync(
                    c => c.SupplierIdentifier == supplierIdentifier && c.CustomerIdentifier == customerIdentifier,
                    token);

            return agreement != null
                ? Result.Success(true)
                : Result.Failure<bool>(ErrorKind.BadRequest, "order.requires.agreement");
        }
        catch (Exception e)
        {
            return Result.Failure<bool>(ErrorKind.Unexpected, "database.error");
        }
    }
}