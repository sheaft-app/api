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
                throw SheaftException.Validation("L'utilisateur est requis.");

            if (value > 5)
                throw SheaftException.Validation("La note ne peut être supérieure à 5.");

            if (value < 0)
                throw SheaftException.Validation("La note ne peut être inférieure à 0.");

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
        public byte[] RowVersion { get; private set; }
    }
}