using System;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Domain
{
    public class CompanyTag
    {
        protected CompanyTag()
        {
        }

        public CompanyTag(Tag tag)
        {
            if (tag == null)
                throw new ValidationException("Le tag pour le producteur est introuvable.");

            Tag = tag;
            TagId = tag.Id;
        }


        public Guid CompanyId { get; private set; }

        public Guid TagId { get; private set; }
        public Tag Tag { get; private set; }
    }
}