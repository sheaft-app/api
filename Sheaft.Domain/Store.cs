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
        private List<StoreTag> _tags;
        private List<TimeSlotHour> _openingHours;

        protected Store()
        {
        }

        public Store(Guid id, string name, string firstname, string lastname, string email, UserAddress address, IEnumerable<TimeSlotHour> openingHours = null, bool openForBusiness = true, string phone = null)
            : base(id, ProfileKind.Store, name, firstname, lastname, email, address, openForBusiness, phone)
        {
            SetOpeningHours(openingHours);
            DomainEvents = new List<DomainEvent>{new StoreRegisteredEvent(Id)};
        }

        public virtual IReadOnlyCollection<StoreTag> Tags => _tags?.AsReadOnly();
        public virtual IReadOnlyCollection<TimeSlotHour> OpeningHours => _openingHours?.AsReadOnly();

        public void SetOpeningHours(IEnumerable<TimeSlotHour> openingHours)
        {
            if (openingHours == null)
                return;

            if (OpeningHours == null)
                _openingHours = new List<TimeSlotHour>();

            _openingHours = openingHours.ToList();
        }

        public void SetTags(IEnumerable<Tag> tags)
        {
            if (tags == null)
                return;

            if (!Tags.Any())
                _tags = new List<StoreTag>();

            _tags = tags.Select(t => new StoreTag(t)).ToList();
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}