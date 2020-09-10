using System;
using Sheaft.Exceptions;
using Sheaft.Interop;
using Sheaft.Interop.Enums;

namespace Sheaft.Domain.Models
{
    public class Rating : IEntity
    {
        protected Rating()
        {
        }

        public Rating(Guid id, decimal value, User user, string comment = null)
        {
            if (user == null)
                throw new ValidationException(MessageKind.Rating_User_Required);

            if (value > 5)
                throw new ValidationException(MessageKind.Rating_CannotBe_GreatherThan, 5);

            if (value < 0)
                throw new ValidationException(MessageKind.Rating_CannotBe_LowerThan, 0);

            Id = id;
            Value = value;
            Comment = comment;
            User = user;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public decimal Value { get; private set; }
        public string Comment { get; private set; }
        public virtual User User { get; private set; }
    }
}