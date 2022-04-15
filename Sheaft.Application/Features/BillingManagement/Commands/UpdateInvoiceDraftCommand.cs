using Sheaft.Domain;
using Sheaft.Domain.InvoiceManagement;

namespace Sheaft.Application.InvoiceManagement;

public record UpdateInvoiceDraftCommand(InvoiceId InvoiceIdentifier, IEnumerable<InvoiceLineDto>? Lines, DateTimeOffset? DueOn) : Command<Result>;

public class UpdateInvoiceDraftHandler : ICommandHandler<UpdateInvoiceDraftCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public UpdateInvoiceDraftHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(UpdateInvoiceDraftCommand request, CancellationToken token)
    {
        var invoiceResult = await _uow.Invoices.Get(request.InvoiceIdentifier, token);
        if (invoiceResult.IsFailure)
            return invoiceResult;

        var invoice = invoiceResult.Value;
        var result = invoice.UpdateLines(request.Lines?.Select(l =>
            InvoiceLine.CreateLine(l.Name, new Quantity(l.Quantity), new UnitPrice(l.UnitPrice), new VatRate(l.Vat))));

        if (result.IsFailure)
            return result;

        if (!request.DueOn.HasValue) 
            return await _uow.Save(token);
        
        var dueResult = invoice.SetDueOn(new InvoiceDueDate(request.DueOn.Value, request.CreatedAt), request.CreatedAt);
        if (dueResult.IsFailure)
            return dueResult;

        return await _uow.Save(token);
    }
}