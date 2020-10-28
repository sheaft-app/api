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