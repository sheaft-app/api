﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Producer;

namespace Sheaft.Domain
{
    public class Producer : Business, IHasDomainEvent
    {
        protected Producer()
        {
        }

        public Producer(Guid id, string name, string firstname, string lastname, string email, UserAddress address, bool openForBusiness = true, string phone = null)
           : base(id, ProfileKind.Producer, name, firstname, lastname, email, address, openForBusiness, phone)
        {
            Tags = new List<ProducerTag>();
            DomainEvents = new List<DomainEvent>{new ProducerRegisteredEvent(Id)};
        }

        public int ProductsCount { get; private set; }
        public bool CanDirectSell { get; private set; }
        public bool NotSubjectToVat { get; private set; }
        public int TagsCount { get; private set; }
        public virtual ICollection<ProducerTag> Tags { get; private set; }

        public void SetTags(IEnumerable<Tag> tags)
        {
            if (tags == null)
                return;

            if (Tags == null || Tags.Any())
                Tags = new List<ProducerTag>();

            Tags = tags.Select(t => new ProducerTag(t)).ToList();
            TagsCount = Tags?.Count ?? 0;
        }

        public void SetNotSubjectToVat(bool notSubjectToVat)
        {
            NotSubjectToVat = notSubjectToVat;
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

        public void IncreaseProductsCount()
        {
            ProductsCount++;
        }
        
        public void DecreaseProductsCount()
        {
            ProductsCount--;
            if (ProductsCount < 0)
                ProductsCount = 0;
        }

        public void SetCanDirectSell(bool canDirectSell)
        {
            CanDirectSell = canDirectSell;
        }
    }
}