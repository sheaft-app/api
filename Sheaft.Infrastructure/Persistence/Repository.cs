using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sheaft.Application.Mediator;
using Sheaft.Domain.Common;
using EventId = Sheaft.Domain.Common.EventId;

namespace Sheaft.Infrastructure.Persistence
{
    public abstract class Repository<T> : IRepository<T>
        where T : class, IEntity
    {
        private readonly IMediator _mediator;
        protected readonly IDbConnectionFactory ContextFactory;
        protected readonly ILogger Logger;

        protected Repository(
            IMediator mediator,
            IDbConnectionFactory contextFactory,
            ILogger logger)
        {
            _mediator = mediator;
            ContextFactory = contextFactory;
            Logger = logger;
        }

        public abstract Task<Result<T>> Get(Guid identifier, CancellationToken token);

        public async Task<Result> Save(T entity, CancellationToken token)
        {
            using var ctx = ContextFactory.CreateConnection();
            ctx.Open();
            using var trx = ctx.BeginTransaction();

            try
            {
                var result = await OnSave(trx, entity, token);
                if (!result.IsSuccess)
                {
                    Logger.LogWarning(
                        "Rollback transaction while saving entity {Name}:{Identifier} because an error occured: {Message}",
                        entity.GetType().Name, entity.Identifier, result.Message);
                    
                    trx.Rollback();
                    return result;
                }

                if(entity is not IAggregateRoot aggregate)
                    trx.Commit();
                else
                {
                    ProcessEvents<IDomainEvent>(aggregate.DomainEvents);
                    trx.Commit();
                    ProcessEvents<IIntegrationEvent>(aggregate.DomainEvents);
                }

                return Result.Success();
            }
            catch (Exception e)
            {
                Logger.LogWarning(
                    "Rollback transaction while saving entity {Name}:{Identifier} because an unexpected error occured: {Message}",
                    entity.GetType().Name, entity.Identifier, e.Message);
                
                trx.Rollback();
                return Result.Error(e.Message);
            }
            finally
            {
                ctx.Close();
            }
        }

        protected abstract Task<Result> OnSave(IDbTransaction transaction, T entity, CancellationToken token);
        protected abstract Task<Result> OnDomainEventProcessed(IDomainEvent domainEvent);

        private void ProcessEvents<U>(IEnumerable<IEvent> eventsToProcess)
            where U : IEvent
        {
            var orderedEvents = eventsToProcess
                .OfType<U>()
                .OrderBy(o => o.OccuredAt)
                .ToList();

            var idx = 0;
            var processedEvents = new EventId[orderedEvents.Count];
            foreach (var orderedEvent in orderedEvents)
            {
                if (processedEvents.Contains(orderedEvent.EventId))
                    continue;

                ProcessEvent(orderedEvent);
                processedEvents[idx++] = orderedEvent.EventId;
            }
        }

        private void ProcessEvent<U>(U orderedEvent) where U : IEvent
        {
            switch (orderedEvent)
            {
                case IDomainEvent domainEvent:
                    ProcessDomainEvent(domainEvent);
                    break;
                case IIntegrationEvent integrationEvent:
                    ProcessIntegrationEvent(integrationEvent);
                    break;
            }
        }

        private void ProcessDomainEvent(IDomainEvent domainEvent)
        {
            OnDomainEventProcessed(domainEvent);
        }

        private void ProcessIntegrationEvent(IIntegrationEvent domainEvent)
        {
            _mediator.Publish(domainEvent);
        }
    }
}