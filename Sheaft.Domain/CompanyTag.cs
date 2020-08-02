using Sheaft.Exceptions;
using Sheaft.Interop.Enums;

namespace Sheaft.Domain.Models
{
    public class CompanyTag
    {
        protected CompanyTag()
        {
        }

        public CompanyTag(Tag tag)
        {
            if (tag == null)
                throw new ValidationException(MessageKind.Company_TagNotFound);

            Tag = tag;
        }

        public virtual Tag Tag { get; private set; }
    }
}