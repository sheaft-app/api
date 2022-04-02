namespace Sheaft.Domain.AgreementManagement;

public interface IAgreementRepository : IRepository<Agreement, AgreementId>
{
    Task<Result<Maybe<Agreement>>> FindAgreementFor(SupplierId supplierIdentifier, CustomerId customerIdentifier, CancellationToken token);
}