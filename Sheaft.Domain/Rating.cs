using System;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Rating : IIdEntity, ITrackCreation, ITrackUpdate
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
            UserId = user.Id;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public decimal Value { get; private set; }
        public string Comment { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid UserId { get; private set; }
        public virtual User User { get; private set; }
    }
}