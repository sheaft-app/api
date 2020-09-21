﻿using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class ConsumerLegal : Legal
    {
        protected ConsumerLegal()
        {
        }

        public ConsumerLegal(Guid id, Consumer consumer, Owner owner)
            : base(id, LegalKind.Natural, owner)
        {
            Consumer = consumer;
        }

        public virtual Consumer Consumer { get; private set; }
    }
}