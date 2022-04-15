using Sheaft.Domain;
using Sheaft.Domain.InvoiceManagement;

namespace Sheaft.Application.InvoiceManagement;

public record UpdateInvoiceDraftCommand(InvoiceId InvoiceIdentifier, IEnumerable<InvoiceLineDto> Lines) : Command<Result>;

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
        var result = invoice.UpdateDraftLines(request.Lines.Select(l =>
            new InvoiceLine(l.Name, new Quantity(l.Quantity), new UnitPrice(l.UnitPrice), new VatRate(l.Vat))));

        if (result.IsFailure)
            return result;

        return await _uow.Save(token);
    }
}