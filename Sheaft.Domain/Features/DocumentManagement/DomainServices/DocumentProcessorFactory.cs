﻿using Sheaft.Domain.CustomerManagement;
using Sheaft.Domain.OrderManagement;

namespace Sheaft.Domain.DocumentManagement;

public interface IDocumentProcessorFactory
{
    IDocumentProcessor GetProcessor(Document document);
}

public class DocumentProcessorFactory : IDocumentProcessorFactory
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IDocumentParamsHandler _documentParamsHandler;
    private readonly IPreparationFileGenerator _preparationFileGenerator;

    public DocumentProcessorFactory(
        IDocumentRepository documentRepository,
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IDocumentParamsHandler documentParamsHandler,
        IPreparationFileGenerator preparationFileGenerator)
    {
        _documentRepository = documentRepository;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _documentParamsHandler = documentParamsHandler;
        _preparationFileGenerator = preparationFileGenerator;
    }
    
    public IDocumentProcessor GetProcessor(Document document)
    {
        if (document.Kind == DocumentKind.Preparation)
            return new PreparationDocumentProcessor(_documentRepository, _orderRepository, _customerRepository,
                _documentParamsHandler, _preparationFileGenerator);

        throw new NotImplementedException();
    }
}