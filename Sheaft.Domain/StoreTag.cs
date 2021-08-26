using System;
using Sheaft.Core.Exceptions;

namespace Sheaft.Domain
{
    public class StoreTag
    {
        protected StoreTag()
        {
        }

        public StoreTag(Tag tag)
        {
            if (tag == null)
                throw SheaftException.Validation("Le tag du magasin est introuvable.");

            Tag = tag;
            TagId = tag.Id;
        }
        
        public Guid StoreId { get; private set; }
        public Guid TagId { get; private set; }
        public virtual Tag Tag { get; private set; }
    }
}