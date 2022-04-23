using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.AgreementManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.OrderManagement;

public class RetrieveDeliveryDays : IRetrieveDeliveryDays
{
    private readonly IDbContext _context;

    public RetrieveDeliveryDays(IDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Maybe<IEnumerable<DeliveryDay>>>> ForAgreementBetween(SupplierId supplierIdentifier,
        CustomerId customerIdentifier, CancellationToken token)
    {
        try
        {
            var agreement = await _context.Set<Agreement>()
                .SingleOrDefaultAsync(
                    c => c.SupplierIdentifier == supplierIdentifier && c.CustomerIdentifier == customerIdentifier,
                    token);

            return agreement != null
                ? Result.Success(Maybe.From(agreement.DeliveryDays.AsEnumerable()))
                : Result.Failure<Maybe<IEnumerable<DeliveryDay>>>(ErrorKind.BadRequest,
                    "no.agreement.between.supplier.and.customer");
        }
        catch (Exception e)
        {
            return Result.Failure<Maybe<IEnumerable<DeliveryDay>>>(ErrorKind.Unexpected, "database.error", e.Message);
        }
    }
}