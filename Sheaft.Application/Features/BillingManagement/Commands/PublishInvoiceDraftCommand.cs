using Sheaft.Domain;
using Sheaft.Domain.BillingManagement;

namespace Sheaft.Application.BillingManagement;

public record PublishInvoiceDraftCommand(InvoiceId InvoiceIdentifier) : Command<Result>;

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
        var invoiceResult = await _publishInvoices.Publish(request.InvoiceIdentifier, request.CreatedAt, token);
        if (invoiceResult.IsFailure)
            return invoiceResult;

        return await _uow.Save(token);
    }
}