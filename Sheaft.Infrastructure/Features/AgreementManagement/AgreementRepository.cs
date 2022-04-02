using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.AgreementManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.AgreementManagement;

internal class AgreementRepository : Repository<Agreement, AgreementId>, IAgreementRepository
{
    public AgreementRepository(IDbContext context)
        : base(context)
    {
    }

    public override Task<Result<Agreement>> Get(AgreementId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .SingleOrDefaultAsync(e => e.Identifier == identifier, token);
            
            return result != null
                ? Result.Success(result)
                : Result.Failure<Agreement>(ErrorKind.NotFound, "agreement.not.found");
        });
    }

    public Task<Result<Maybe<Agreement>>> FindAgreementFor(SupplierId supplierIdentifier, CustomerId customerIdentifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            return Result.Success(await Values
                .SingleOrDefaultAsync(e => e.CustomerIdentifier == customerIdentifier && e.SupplierIdentifier == supplierIdentifier, token) ?? Maybe<Agreement>.None);
        });
    }
}