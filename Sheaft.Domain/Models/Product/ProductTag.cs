using System;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Domain
{
    public class ProductTag
    {
        protected ProductTag()
        {
        }

        public ProductTag(Tag tag)
        {
            if (tag == null)
                throw new ValidationException("Le tag du produit est introuvable.");

            Tag = tag;
            TagId = tag.Id;
        }

        public Guid ProductId { get; private set; }

        public Guid TagId { get; private set; }
        public virtual Tag Tag { get; private set; }
    }
}