using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain.Models
{
    public class Producer : Business
    {
        private List<ProducerTag> _tags;

        protected Producer()
        {
        }

        public Producer(Guid id, string name, string firstname, string lastname, string email, UserAddress address, bool openForBusiness = true, string phone = null, string description = null)
           : base(id, ProfileKind.Producer, name, firstname, lastname, email, address, openForBusiness, phone, description)
        {
        }

        public virtual IReadOnlyCollection<ProducerTag> Tags => _tags?.AsReadOnly(); 

        public void SetTags(IEnumerable<Tag> tags)
        {
            if (tags == null)
                return;

            if (!Tags.Any())
                _tags = new List<ProducerTag>();

            _tags = tags.Select(t => new ProducerTag(t)).ToList();
        }
    }
}