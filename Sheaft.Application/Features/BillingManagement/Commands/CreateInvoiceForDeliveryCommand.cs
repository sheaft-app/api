using Sheaft.Domain;
using Sheaft.Domain.BillingManagement;

namespace Sheaft.Application.BillingManagement;

public record CreateInvoiceForDeliveryCommand(DeliveryId DeliveryIdentifier) : Command<Result<string>>;

public class CreateInvoiceForDeliveryHandler : ICommandHandler<CreateInvoiceForDeliveryCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICreateInvoices _createInvoices;

    public CreateInvoiceForDeliveryHandler(
        IUnitOfWork uow,
        ICreateInvoices createInvoices)
    {
        _uow = uow;
        _createInvoices = createInvoices;
    }

    public async Task<Result<string>> Handle(CreateInvoiceForDeliveryCommand request, CancellationToken token)
    {
        var invoiceResult = await _createInvoices.CreateForDelivery(request.DeliveryIdentifier, token);
        if (invoiceResult.IsFailure)
            return Result.Failure<string>(invoiceResult);
        
        var result = await _uow.Save(token);
        return result.IsSuccess
            ? Result.Success(invoiceResult.Value.Identifier.Value)
            : Result.Failure<string>(result);
    }
}