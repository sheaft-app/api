using System;
using System.Collections.Generic;
using Sheaft.Core;

namespace Sheaft.Domain.Models.Common
{
    public interface IHasDomainEvent
    {
        public List<DomainEvent> DomainEvents { get; set; }
    }

    public abstract class DomainEvent
    {
        protected DomainEvent(RequestUser requestUser)
        {
            DateOccurred = DateTimeOffset.UtcNow;
            RequestUser = requestUser;
        }

        public RequestUser RequestUser { get; protected set; }
        
        public bool IsPublished { get; set; }
        public DateTimeOffset DateOccurred { get; protected set; }
    }
}