using System;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class Points
    {
        protected Points()
        {
        }

        public Points(Guid id, PointKind kind, int quantity, DateTimeOffset createdOn)
        {
            if (quantity < 0)
                throw new ValidationException(MessageKind.Points_Quantity_CannotBe_LowerThan, 0);

            Id = id;
            Kind = kind;
            Quantity = quantity;
            CreatedOn = createdOn;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public PointKind Kind { get; private set; }
        public int Quantity { get; private set; }
    }
}