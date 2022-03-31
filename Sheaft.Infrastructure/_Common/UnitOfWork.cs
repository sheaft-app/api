using MediatR;
using Microsoft.Extensions.Logging;
using Sheaft.Application;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.AgreementManagement;
using Sheaft.Domain.ProductManagement;
using Sheaft.Domain.CustomerManagement;
using Sheaft.Domain.SupplierManagement;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.AgreementManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Infrastructure.ProductManagement;
using Sheaft.Infrastructure.CustomerManagement;
using Sheaft.Infrastructure.SupplierManagement;

namespace Sheaft.Infrastructure;

internal class UnitOfWork : IUnitOfWork
{
    private readonly IMediator _mediator;
    private readonly AppDbContext _context;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(
        IMediator mediator,
        AppDbContext context,
        ILogger<UnitOfWork> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;

        Accounts = new AccountRepository(_context);
        
        Suppliers = new SupplierRepository(_context);
        Customers = new CustomerRepository(_context);
        
        Catalogs = new CatalogRepository(_context);
        Products = new ProductRepository(_context);
        
        Agreements = new AgreementRepository(_context);
    }

    public IAccountRepository Accounts { get; }
    
    public ISupplierRepository Suppliers { get; }
    public ICustomerRepository Customers { get; }
    
    public ICatalogRepository Catalogs { get; }
    public IProductRepository Products { get; }
    
    public IAgreementRepository Agreements { get; }


    public async Task<Result<int>> Save(CancellationToken token)
    {
        var transaction = _context.Database.CurrentTransaction;
        
        try
        {
            var events = _context.ChangeTracker
                .Entries<IAggregateRoot>()
                .Where(e => e.Entity.Events.Any())
                .SelectMany(e => e.Entity.Events)
                .OrderBy(e => e.Key)
                .Select(e => e.Value)
                .ToList();

            var result = await _context.SaveChangesAsync(token);
            if (transaction != null)
                await transaction.CommitAsync(token);

            ProcessDomainEvents(events);

            return Result.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);

            if (transaction != null)
                await transaction.RollbackAsync(token);

            return Result.Failure<int>(ErrorKind.Unexpected, e.Message);
        }
    }

    private void ProcessDomainEvents(List<IDomainEvent> eventsToProcess)
    {
        try
        {
            var idx = 0;
            var processedEvents = new Guid[eventsToProcess.Count];
            foreach (var orderedEvent in eventsToProcess)
            {
                if (processedEvents.Contains(orderedEvent.EventId))
                    continue;

                ProcessDomainEvent(orderedEvent);
                processedEvents[idx++] = orderedEvent.EventId;
            }
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, e.Message);
        }
    }

    private void ProcessDomainEvent(IDomainEvent domainEvent)
    {
        if (domainEvent.Published)
            return;

        _mediator.Publish(domainEvent);
        domainEvent.Published = true;
    }
}