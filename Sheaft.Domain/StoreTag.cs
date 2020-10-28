using Sheaft.Exceptions;
using Sheaft.Domain.Enums;

namespace Sheaft.Domain.Models
{
    public class StoreTag
    {
        protected StoreTag()
        {
        }

        public StoreTag(Tag tag)
        {
            if (tag == null)
                throw new ValidationException(MessageKind.User_TagNotFound);

            Tag = tag;
        }

        public virtual Tag Tag { get; private set; }
    }
}