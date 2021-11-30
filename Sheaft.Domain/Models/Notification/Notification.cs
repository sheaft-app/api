using System;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Notification : IIdEntity, ITrackCreation, ITrackUpdate
    {
        protected Notification()
        {
        }

        public Notification(string method, string content, User user)
        {
            Method = method;
            User = user;
            UserId = user.Id;
            CompanyId = user.CompanyId;
            Content = content ?? throw new ValidationException("Le contenu de la notification est requis.");
        }

        public Guid Id { get; } = Guid.NewGuid();
        public bool Unread { get; set; } = true;
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public string Content { get; private set; }
        public string Method { get; private set; }
        public Guid UserId { get; private set; }
        public Guid? CompanyId { get; private set; }
        public User User { get; private set; }
    }
}