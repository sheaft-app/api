using Sheaft.Domain.Common;

namespace Sheaft.Domain.AccountManagement.Events;

public record AccountLoggedIn(EntityId AccountId) : Event, IDomainEvent, IIntegrationEvent;