using System;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Domain
{
    public class ConsumerLegal : Legal
    {
        protected ConsumerLegal()
        {
        }

        public ConsumerLegal(Guid id, Consumer consumer, Owner owner)
            : base(id, LegalKind.Natural, consumer, owner)
        {
        }

        public override void SetKind(LegalKind kind)
        {
            if (kind != LegalKind.Natural)
                throw new ValidationException(MessageKind.Legal_Kind_Must_Be_Natural);

            base.SetKind(kind);
        }

    }
}