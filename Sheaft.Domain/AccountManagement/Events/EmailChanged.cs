using Sheaft.Domain.Common;

namespace Sheaft.Domain.AccountManagement.Events;

public record EmailChanged(EntityId AccountId, string OldEmail, string NewEmail) : Event, IIntegrationEvent;