using Sheaft.Domain.Common;

namespace Sheaft.Domain.AccountManagement.Events;

public record PasswordChanged(EntityId AccountId) : Event, IIntegrationEvent;