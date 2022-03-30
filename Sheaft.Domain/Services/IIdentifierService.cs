namespace Sheaft.Domain;

public interface IIdentifierService
{
    Task<Result<int>> GetNextPurchaseOrderReference(ProfileId profileIdentifier, CancellationToken token);
    Task<Result<int>> GetNextDeliveryReference(ProfileId profileIdentifier, CancellationToken token);
}