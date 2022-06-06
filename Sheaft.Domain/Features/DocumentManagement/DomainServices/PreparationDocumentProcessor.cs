using Sheaft.Domain.CustomerManagement;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Domain.DocumentManagement;

public interface IDocumentProcessor
{
    Task<Result> Process(DocumentId documentIdentifier, CancellationToken token);
}

public class PreparationDocumentProcessor : IDocumentProcessor
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IDocumentParamsHandler _documentParamsHandler;
    private readonly IPreparationFileGenerator _preparationFileGenerator;
    private readonly IFileStorage _fileStorage;

    public PreparationDocumentProcessor(
        IDocumentRepository documentRepository,
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IDocumentParamsHandler documentParamsHandler,
        IPreparationFileGenerator preparationFileGenerator,
        IFileStorage fileStorage)
    {
        _documentRepository = documentRepository;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _documentParamsHandler = documentParamsHandler;
        _preparationFileGenerator = preparationFileGenerator;
        _fileStorage = fileStorage;
    }

    public async Task<Result> Process(DocumentId documentIdentifier, CancellationToken token)
    {
        var documentResult = await _documentRepository.Get(documentIdentifier, token);
        if (documentResult.IsFailure)
            return documentResult;

        var document = documentResult.Value;
        var processResult = await ProcessDocument(document, token);
        if (processResult.IsFailure)
            document.SetProcessingError(processResult.Error.Code);

        _documentRepository.Update(document);
        return Result.Success();
    }

    private async Task<Result> ProcessDocument(Document document, CancellationToken token)
    {
        var documentParams = document.GetParams<PreparationDocumentParams>(_documentParamsHandler);

        document.StartProcessing();

        var ordersResult = await _orderRepository.Get(documentParams.OrderIdentifiers, token);
        if (ordersResult.IsFailure)
            return ordersResult;

        var productsResult = GetPreparationProductsToPrepare(ordersResult.Value);
        if (productsResult.IsFailure)
            return productsResult;

        var clientsResult = await GetPreparationClientsToPrepare(ordersResult.Value, token);
        if (clientsResult.IsFailure)
            return clientsResult;

        var products = productsResult.Value;
        var clients = clientsResult.Value;
        var documentData = new PreparationDocumentData(products, clients);

        var generationResult = await _preparationFileGenerator.Generate(documentData, token);
        if (generationResult.IsFailure)
            return generationResult;

        var saveResult = await _fileStorage.SaveDocument(document, generationResult.Value, token);
        if (saveResult.IsFailure)
            return saveResult;

        document.CompleteProcessing();
        return Result.Success();
    }

    private async Task<Result<IEnumerable<ClientOrdersToPrepare>>> GetPreparationClientsToPrepare(
        IEnumerable<Order> orders, CancellationToken token)
    {
        var customerIdentifiers = orders.Select(o => o.CustomerId).Distinct().ToList();
        var customersNameResult = await _customerRepository.GetInfo(customerIdentifiers, token);
        if (customersNameResult.IsFailure)
            return Result.Failure<IEnumerable<ClientOrdersToPrepare>>(customersNameResult);

        try
        {
            var clients = orders.GroupBy(o => o.CustomerId)
                .Select(o =>
                    new ClientOrdersToPrepare(
                        customersNameResult.Value.Single(c => c.Identifier == o.Key).Name,
                        o.Select(l => l.Reference).ToList()))
                .ToList();

            return Result.Success(clients.AsEnumerable());
        }
        catch (Exception e)
        {
            return Result.Failure<IEnumerable<ClientOrdersToPrepare>>(ErrorKind.Unexpected, "preparation.clients.error");
        }
    }

    private Result<IEnumerable<ProductToPrepare>> GetPreparationProductsToPrepare(IEnumerable<Order> orders)
    {
        try
        {
            var productsPerOrders = new List<ProductPerOrderToPrepare>();
            foreach (var order in orders)
            {
                var orderLines = order.Lines.Where(l => l.LineKind == OrderLineKind.Product).ToList();
                productsPerOrders.AddRange(orderLines
                    .Select(l =>
                        new ProductPerOrderToPrepare(
                            new ProductReference(l.Identifier),
                            new ProductName(l.Name),
                            l.Quantity,
                            order.CustomerId,
                            order.Reference)).ToList());
            }

            var products = productsPerOrders
                .GroupBy(o => new {o.ProductReference, o.Name})
                .Select(l =>
                    new ProductToPrepare(
                        l.Key.ProductReference, 
                        l.Key.Name,
                        l.Select(p => new QuantityPerOrder(p.OrderReference, p.Quantity)).ToList()))
                .ToList();

            return Result.Success(products.AsEnumerable());
        }
        catch (Exception e)
        {
            return Result.Failure<IEnumerable<ProductToPrepare>>(ErrorKind.Unexpected, "preparation.products.error");
        }
    }

    private record ProductPerOrderToPrepare(ProductReference ProductReference, ProductName Name, Quantity Quantity,
        CustomerId CustomerIdentifier, OrderReference OrderReference);
}

public record PreparationDocumentData(IEnumerable<ProductToPrepare> Products, IEnumerable<ClientOrdersToPrepare> Clients);

public record ProductToPrepare(ProductReference ProductReference, ProductName Name, IEnumerable<QuantityPerOrder> QuantityPerOrder);

public record QuantityPerOrder(OrderReference OrderReference, Quantity Quantity);

public record ClientOrdersToPrepare(string ClientName, IEnumerable<OrderReference> Orders)
{
    public int OrdersCount => Orders.Count();
}