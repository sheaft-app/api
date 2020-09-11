using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain.Models
{
    public class Store : Company
    {
        private List<StoreTag> _tags;
        private List<TimeSlotHour> _openingHours;

        protected Store()
        {
        }

        public Store(Guid id, string name, string firstname, string lastname, string email, string siret, string vatIdentifier, UserAddress address, IEnumerable<TimeSlotHour> openingHours = null, bool openForBusiness = true, string phone = null, string description = null)
            : base(id, ProfileKind.Store, LegalKind.Business, name, firstname, lastname, email, siret, vatIdentifier, address, openForBusiness, phone, description)
        {
            SetOpeningHours(openingHours);
        }

        public virtual IReadOnlyCollection<StoreTag> Tags { get { return _tags.AsReadOnly(); } }
        public virtual IReadOnlyCollection<TimeSlotHour> OpeningHours { get { return _openingHours.AsReadOnly(); } }

        public void SetOpeningHours(IEnumerable<TimeSlotHour> openingHours)
        {
            if (openingHours == null)
                return;

            if (!OpeningHours.Any())
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
    }
}