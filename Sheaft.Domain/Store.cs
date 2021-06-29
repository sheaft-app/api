using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Store;

namespace Sheaft.Domain
{
    public class Store : Business, IHasDomainEvent
    {
        protected Store()
        {
        }

        public Store(Guid id, string name, string firstname, string lastname, string email, UserAddress address, IEnumerable<OpeningHours> openingHours = null, bool openForBusiness = true, string phone = null)
            : base(id, ProfileKind.Store, name, firstname, lastname, email, address, openForBusiness, phone)
        {
            Tags = new List<StoreTag>();
            OpeningHours = new List<OpeningHours>();
            SetOpeningHours(openingHours);
            DomainEvents = new List<DomainEvent>{new StoreRegisteredEvent(Id)};
        }

        public int TagsCount { get; private set; }
        public int ProducersCount { get; private set; }
        public int OpeningHoursCount { get; private set; }
        public virtual IReadOnlyCollection<StoreTag> Tags { get; private set; }
        public virtual IReadOnlyCollection<OpeningHours> OpeningHours { get; private set; }

        public void IncreaseProducersCount()
        {
            ProducersCount++;
        }
        
        public void DecreaseProducersCount()
        {
            ProducersCount--;
            if (ProducersCount < 0)
                ProducersCount = 0;
        }
        public void SetOpeningHours(IEnumerable<OpeningHours> openingHours)
        {
            if (openingHours == null)
                return;

            if (OpeningHours == null || OpeningHours.Any())
                OpeningHours = new List<OpeningHours>();

            OpeningHours = openingHours.ToList();
            OpeningHoursCount = OpeningHours?.Count ?? 0;
        }

        public void SetTags(IEnumerable<Tag> tags)
        {
            if (tags == null)
                return;

            if (Tags == null || Tags.Any())
                Tags = new List<StoreTag>();

            Tags = tags.Select(t => new StoreTag(t)).ToList();
            TagsCount = Tags?.Count ?? 0;
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}