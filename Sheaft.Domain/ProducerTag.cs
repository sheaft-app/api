using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;

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
                throw new ValidationException(MessageKind.User_TagNotFound);

            Tag = tag;
        }

        public virtual Tag Tag { get; private set; }
    }
}