using System;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;

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
                throw SheaftException.Validation("Le statut légal de l'entité doit être de type personnel.");

            base.SetKind(kind);
        }

    }
}