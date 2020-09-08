using Sheaft.Exceptions;
using Sheaft.Interop.Enums;

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
                throw new ValidationException(MessageKind.Company_TagNotFound);

            Tag = tag;
        }

        public virtual Tag Tag { get; private set; }
    }
}