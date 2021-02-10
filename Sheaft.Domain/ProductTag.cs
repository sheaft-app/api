using Sheaft.Domain.Enum;
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
                throw new ValidationException(MessageKind.Product_TagNotFound);

            Tag = tag;
        }

        public virtual Tag Tag { get; private set; }
    }
}