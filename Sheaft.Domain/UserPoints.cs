using Sheaft.Exceptions;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class UserPoints
    {
        protected UserPoints()
        {
        }

        public UserPoints(PointKind kind, int quantity, DateTimeOffset createdOn)
        {
            if (quantity < 0)
                throw new ValidationException(MessageKind.UserPoints_Quantity_CannotBe_LowerThan, 0);

            Kind = kind;
            Quantity = quantity;
            CreatedOn = createdOn;
        }

        public DateTimeOffset CreatedOn { get; private set; }
        public PointKind Kind { get; private set; }
        public int Quantity { get; private set; }
    }
}