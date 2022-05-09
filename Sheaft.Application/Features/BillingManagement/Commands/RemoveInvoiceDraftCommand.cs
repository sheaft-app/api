using Sheaft.Domain;

namespace Sheaft.Application.BillingManagement;

public record RemoveInvoiceDraftCommand(InvoiceId InvoiceIdentifier) : Command<Result>;

public class RemoveInvoiceDraftHandler : ICommandHandler<RemoveInvoiceDraftCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public RemoveInvoiceDraftHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(RemoveInvoiceDraftCommand request, CancellationToken token)
    {
        var invoiceResult = await _uow.Invoices.Get(request.InvoiceIdentifier, token);
        if (invoiceResult.IsFailure)
            return invoiceResult;

        var invoice = invoiceResult.Value;
        if (invoice.Status != InvoiceStatus.Draft)
            return Result.Failure(ErrorKind.BadRequest, "invoice.remove.requires.draft");
        
        var ordersResult = await _uow.Orders.Find(invoice.Identifier, token);
        if (ordersResult.IsFailure)
            return Result.Failure(ordersResult);

        foreach (var order in ordersResult.Value)
        {
            order.DetachInvoice();
            _uow.Orders.Update(order);
        }

        var invoiceWithCreditNotesResult = await _uow.Invoices.GetInvoiceWithCreditNote(invoice.Identifier, token);
        if (invoiceWithCreditNotesResult.IsFailure)
            return Result.Failure(invoiceWithCreditNotesResult);

        var invoiceWithCreditNote = invoiceWithCreditNotesResult.Value;
        if(invoiceWithCreditNote.HasValue)
            invoiceWithCreditNote.Value.RemoveCreditNote(invoice);
        
        _uow.Invoices.Remove(invoice);
        return await _uow.Save(token);
    }
}