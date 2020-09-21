using Sheaft.Exceptions;
using Sheaft.Domain.Enums;

namespace Sheaft.Domain.Models
{

    public class ProducerTag
    {
        protected ProducerTag()
        {
        }

        public ProducerTag(Tag tag)
        {
            if (tag == null)
                throw new ValidationException(MessageKind.Business_TagNotFound);

            Tag = tag;
        }

        public virtual Tag Tag { get; private set; }
    }
}