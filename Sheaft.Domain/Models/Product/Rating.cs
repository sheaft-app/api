using System;
using Sheaft.Domain.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Rating : IIdEntity, ITrackCreation
    {
        protected Rating()
        {
        }

        public Rating(decimal value, User user, string comment = null)
        {
            if (user == null)
                throw new ValidationException("L'utilisateur est requis.");

            if (value > 5)
                throw new ValidationException("La note ne peut être supérieure à 5.");

            if (value < 0)
                throw new ValidationException("La note ne peut être inférieure à 0.");

            Value = value;
            Comment = comment;
            UserId = user.Id;
            Username = user.Name;
            Email = user.Email;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public DateTimeOffset CreatedOn { get; private set; }
        public decimal Value { get; private set; }
        public string Comment { get; private set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Guid ProductId { get; private set; }
        public Guid UserId { get; private set; }
    }
}