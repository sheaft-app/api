using Sheaft.Exceptions;
using Sheaft.Domain.Enums;

namespace Sheaft.Domain.Models
{
    public class ProductTag
    {
        protected ProductTag()
        {
        }

        public ProductTag(Tag tag)
        {
            if (tag == null)
                throw new ValidationException(MessageKind.Product_TagNotFound);

            Tag = tag;
        }

        public virtual Tag Tag { get; private set; }
    }
}