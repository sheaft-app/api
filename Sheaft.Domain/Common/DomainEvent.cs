﻿using System;
using System.Collections.Generic;

namespace Sheaft.Domain.Events
{
    public interface IHasDomainEvent
    {
        public List<DomainEvent> DomainEvents { get; }
    }

    public abstract class DomainEvent
    {
        protected DomainEvent()
        {
            DateOccurred = DateTimeOffset.UtcNow;
        }
        
        public bool IsPublished { get; set; }
        public DateTimeOffset DateOccurred { get; }
    }
}