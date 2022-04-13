using Sheaft.Domain;
using Sheaft.Domain.InvoiceManagement;

namespace Sheaft.Application.InvoiceManagement;

public record UpdateInvoiceDraftLinesCommand
    (InvoiceId InvoiceIdentifier, IEnumerable<InvoiceLineDto> Lines) : Command<Result>;

public class UpdateInvoiceDraftLinesHandler : ICommandHandler<UpdateInvoiceDraftLinesCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public UpdateInvoiceDraftLinesHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(UpdateInvoiceDraftLinesCommand request, CancellationToken token)
    {
        var invoiceResult = await _uow.Invoices.Get(request.InvoiceIdentifier, token);
        if (invoiceResult.IsFailure)
            return invoiceResult;

        var invoice = invoiceResult.Value;
        var result = invoice.UpdateDraftLines(request.Lines.Select(l =>
            new InvoiceLine(l.Name, new Quantity(l.Quantity), new UnitPrice(l.UnitPrice), new VatRate(l.Vat))));

        if (result.IsFailure)
            return result;

        return await _uow.Save(token);
    }
}