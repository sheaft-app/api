using Sheaft.Domain;
using Sheaft.Domain.BillingManagement;

namespace Sheaft.Application.BillingManagement;

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
        var creditNoteResult = await _cancelInvoices.Cancel(request.InvoiceIdentifier, request.Reason, request.CreatedAt, token);
        if (creditNoteResult.IsFailure)
            return creditNoteResult;
        
        var saveResult = await _uow.Save(token);
        return saveResult.IsFailure ? Result.Failure<string>(saveResult) : creditNoteResult;
    }
}