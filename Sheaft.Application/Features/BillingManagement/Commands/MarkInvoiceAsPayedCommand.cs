﻿using Sheaft.Domain;

namespace Sheaft.Application.BillingManagement;

public record MarkInvoiceAsPayedCommand(InvoiceId InvoiceIdentifier, string Reference, DateTimeOffset PayedOn, PaymentKind Kind) : Command<Result>;

public class MarkInvoiceAsPayedHandler : ICommandHandler<MarkInvoiceAsPayedCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public MarkInvoiceAsPayedHandler(
        IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(MarkInvoiceAsPayedCommand request, CancellationToken token)
    {
        var invoiceResult = await _uow.Invoices.Get(request.InvoiceIdentifier, token);
        if (invoiceResult.IsFailure)
            return invoiceResult;

        var invoice = invoiceResult.Value;
        var result = invoice.MarkAsPayed(new PaymentReference(request.Reference), request.Kind, request.PayedOn);
        if (result.IsFailure)
            return result;
        
        _uow.Invoices.Update(invoice);
        return await _uow.Save(token);
    }
}