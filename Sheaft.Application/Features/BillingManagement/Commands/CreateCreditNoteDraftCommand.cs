using Sheaft.Domain;
using Sheaft.Domain.BillingManagement;

namespace Sheaft.Application.BillingManagement;

public record CreateCreditNoteDraftCommand(SupplierId SupplierIdentifier, CustomerId CustomerIdentifier) : Command<Result<string>>;

public class CreateCreditNoteDraftHandler : ICommandHandler<CreateCreditNoteDraftCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;

    public CreateCreditNoteDraftHandler(
        IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<string>> Handle(CreateCreditNoteDraftCommand request, CancellationToken token)
    {
        var result = await _uow.Save(token);
        return result.IsSuccess
            ? Result.Success(string.Empty)
            : Result.Failure<string>(result);
    }
}