using Sheaft.Exceptions;
using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class Points
    {
        protected Points()
        {
        }

        public Points(User user, PointKind kind, int quantity, DateTimeOffset createdOn)
        {
            if (quantity < 0)
                throw new ValidationException(MessageKind.UserPoints_Quantity_CannotBe_LowerThan, 0);

            Kind = kind;
            Quantity = quantity;
            CreatedOn = createdOn;
            User = user;
        }

        public DateTimeOffset CreatedOn { get; private set; }
        public PointKind Kind { get; private set; }
        public int Quantity { get; private set; }
        public virtual User User { get; private set; }
    }
}