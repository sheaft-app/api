using System.Diagnostics;
using Sheaft.Domain;
using Sheaft.Domain.Email;

namespace Sheaft.Application.BillingManagement;

public record SendInvoiceCommand(InvoiceId InvoiceIdentifier) : Command<Result>;

public class SendInvoiceHandler : ICommandHandler<SendInvoiceCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IEmailingService _emailingService;

    public SendInvoiceHandler(
        IUnitOfWork uow,
        IEmailingService emailingService)
    {
        _uow = uow;
        _emailingService = emailingService;
    }

    public async Task<Result> Handle(SendInvoiceCommand request, CancellationToken token)
    {
        var invoiceResult = await _uow.Invoices.Get(request.InvoiceIdentifier, token);
        if (invoiceResult.IsFailure)
            return invoiceResult;

        var invoice = invoiceResult.Value;
        var result = invoice.MarkAsSent(request.CreatedAt);
        if (result.IsFailure)
            return result;

        Debug.Assert(invoice.PublishedOn != null, "invoice.PublishedOn != null");
        
        var emailResult = await _emailingService.SendTemplatedEmail(invoice.Customer.Email.Value, invoice.Customer.Name,
            $"{(invoice.Kind == InvoiceKind.Invoice ? "Facture" : "Avoir")} n°{invoice.Identifier.Value} du {invoice.PublishedOn.Value:d}",
            invoice.Kind == InvoiceKind.Invoice ? EmailTemplates.Invoice : EmailTemplates.CreditNote, new { }, true,
            token);

        if (emailResult.IsFailure)
            return emailResult;

        _uow.Invoices.Update(invoice);
        return await _uow.Save(token);
    }
}