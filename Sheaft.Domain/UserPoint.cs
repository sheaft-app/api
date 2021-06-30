using System;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class UserPoint: IIdEntity, ITrackCreation
    {
        protected UserPoint()
        {
        }

        public UserPoint(User user, Guid id, PointKind kind, int quantity, DateTimeOffset createdOn)
        {
            if (quantity < 0)
                throw SheaftException.Validation("Le nombre de point doit être supérieur à 0.");

            Id = id;
            Kind = kind;
            Quantity = quantity;
            CreatedOn = createdOn;
            UserId = user.Id;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public PointKind Kind { get; private set; }
        public int Quantity { get; private set; }

        public Guid UserId { get; private set; }
    }
}