using System;
using Sheaft.Core.Exceptions;

namespace Sheaft.Domain
{
    public class ProducerTag
    {
        protected ProducerTag()
        {
        }

        public ProducerTag(Tag tag)
        {
            if (tag == null)
                throw SheaftException.Validation("Le tag pour le producteur est introuvable.");

            Tag = tag;
            TagId = tag.Id;
        }


        public Guid ProducerId { get; private set; }

        public Guid TagId { get; private set; }
        public virtual Tag Tag { get; private set; }
    }
}