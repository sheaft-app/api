using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events.Producer;

namespace Sheaft.Domain
{
    public class Producer : Business, IHasDomainEvent
    {
        private List<ProducerTag> _tags;

        protected Producer()
        {
        }

        public Producer(Guid id, string name, string firstname, string lastname, string email, UserAddress address, bool openForBusiness = true, string phone = null, string description = null)
           : base(id, ProfileKind.Producer, name, firstname, lastname, email, address, openForBusiness, phone, description)
        {
            DomainEvents = new List<DomainEvent>{new ProducerRegisteredEvent(Id)};
        }

        public virtual IReadOnlyCollection<ProducerTag> Tags => _tags?.AsReadOnly();

        public bool HasProducts { get; set; }
        public bool CanDirectSell { get; set; }
        public bool NotSubjectToVat { get; private set; }

        public void SetTags(IEnumerable<Tag> tags)
        {
            if (tags == null)
                return;

            if (!Tags.Any())
                _tags = new List<ProducerTag>();

            _tags = tags.Select(t => new ProducerTag(t)).ToList();
        }

        public void SetNotSubjectToVat(bool notSubjectToVat)
        {
            NotSubjectToVat = notSubjectToVat;
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}