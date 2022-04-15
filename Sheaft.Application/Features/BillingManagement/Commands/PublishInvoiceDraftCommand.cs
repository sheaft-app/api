using Sheaft.Domain;
using Sheaft.Domain.BillingManagement;
using Sheaft.Domain.InvoiceManagement;

namespace Sheaft.Application.InvoiceManagement;

public record PublishInvoiceDraftCommand(InvoiceId InvoiceIdentifier, DateTimeOffset DueOn, IEnumerable<InvoiceLineDto>? Lines = null) : Command<Result>;

public class PublishInvoiceDraftHandler : ICommandHandler<PublishInvoiceDraftCommand, Result>
{
    private readonly IUnitOfWork _uow;
    private readonly IPublishInvoices _publishInvoices;

    public PublishInvoiceDraftHandler(
        IUnitOfWork uow,
        IPublishInvoices publishInvoices)
    {
        _uow = uow;
        _publishInvoices = publishInvoices;
    }

    public async Task<Result> Handle(PublishInvoiceDraftCommand request, CancellationToken token)
    {
        var invoiceResult = await _publishInvoices.Publish(request.InvoiceIdentifier,
            new InvoiceDueDate(request.DueOn),
            request.Lines?.Select(l =>
                new InvoiceLine(l.Name, new Quantity(l.Quantity), new UnitPrice(l.UnitPrice), new VatRate(l.Vat))),
            request.CreatedAt,
            token);

        if (invoiceResult.IsFailure)
            return invoiceResult;

        return await _uow.Save(token);
    }
}