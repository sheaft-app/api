using Sheaft.Domain;
using Sheaft.Domain.BillingManagement;

namespace Sheaft.Application.InvoiceManagement;

public record CancelInvoiceCommand(InvoiceId InvoiceIdentifier, string Reason) : Command<Result<string>>;

public class CancelInvoiceHandler : ICommandHandler<CancelInvoiceCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICancelInvoices _cancelInvoices;

    public CancelInvoiceHandler(
        IUnitOfWork uow,
        ICancelInvoices cancelInvoices)
    {
        _uow = uow;
        _cancelInvoices = cancelInvoices;
    }

    public async Task<Result<string>> Handle(CancelInvoiceCommand request, CancellationToken token)
    {
        var result = await _cancelInvoices.Cancel(request.InvoiceIdentifier, request.Reason, request.CreatedAt, token);
        if (result.IsFailure)
            return result;
        
        var saveResult = await _uow.Save(token);
        if (saveResult.IsFailure)
            return Result.Failure<string>(saveResult);

        return result;
    }
}