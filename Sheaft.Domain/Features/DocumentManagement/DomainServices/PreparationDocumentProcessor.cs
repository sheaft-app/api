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
    private readonly IFileProvider _fileProvider;

    public PreparationDocumentProcessor(
        IDocumentRepository documentRepository,
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IDocumentParamsHandler documentParamsHandler,
        IPreparationFileGenerator preparationFileGenerator,
        IFileProvider fileProvider)
    {
        _documentRepository = documentRepository;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _documentParamsHandler = documentParamsHandler;
        _preparationFileGenerator = preparationFileGenerator;
        _fileProvider = fileProvider;
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

        var startResult = document.StartProcessing();
        if (startResult.IsFailure)
            return startResult;

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
        var documentData = new PreparationDocumentData(products, clients, products.Count(), clients.Count());

        var generationResult = await _preparationFileGenerator.Generate(documentData, token);
        if (generationResult.IsFailure)
            return generationResult;

        var saveResult = await _fileProvider.SaveDocument(document.SupplierIdentifier, document.Identifier,
            generationResult.Value, token);
        if (saveResult.IsFailure)
            return saveResult;

        var completionResult = document.CompleteProcessing(saveResult.Value);
        return completionResult.IsFailure ? completionResult : Result.Success();
    }

    private async Task<Result<IEnumerable<ClientOrdersToPrepare>>> GetPreparationClientsToPrepare(
        IEnumerable<Order> orders, CancellationToken token)
    {
        var customerIdentifiers = orders.Select(o => o.CustomerIdentifier).Distinct().ToList();
        var customersNameResult = await _customerRepository.GetInfo(customerIdentifiers, token);
        if (customersNameResult.IsFailure)
            return Result.Failure<IEnumerable<ClientOrdersToPrepare>>(customersNameResult);

        try
        {
            var clients = orders.GroupBy(o => o.CustomerIdentifier)
                .Select(o =>
                    new ClientOrdersToPrepare(
                        o.Key,
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
                            order.CustomerIdentifier,
                            order.Reference)).ToList());
            }

            var products = productsPerOrders
                .GroupBy(o => new {o.CustomerIdentifier, o.ProductReference, o.Name})
                .Select(l =>
                    new ProductToPrepare(l.Key.ProductReference, l.Key.Name, new Quantity(l.Sum(p => p.Quantity.Value)),
                        l.Key.CustomerIdentifier,
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

public record PreparationDocumentData(IEnumerable<ProductToPrepare> Products,
    IEnumerable<ClientOrdersToPrepare> Clients, int ProductsCount, int ClientsCount);

public record ProductToPrepare(ProductReference ProductReference, ProductName Name, Quantity Quantity,
    CustomerId CustomerIdentifier, IEnumerable<QuantityPerOrder> QuantityPerOrder);

public record QuantityPerOrder(OrderReference OrderReference, Quantity Quantity);

public record ClientOrdersToPrepare(CustomerId CustomerIdentifier, string ClientName,
    IEnumerable<OrderReference> Orders);