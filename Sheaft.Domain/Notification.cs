using System;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Notification : IIdEntity, ITrackCreation, ITrackUpdate

    {
        protected Notification()
        {
        }

        public Notification(Guid id, NotificationKind kind, string method, string content, User user)
        {
            Id = id;
            Kind = kind;
            Method = method;
            User = user;
            UserId = user.Id;
            Content = content ?? throw new ValidationException(MessageKind.Notification_Require_Content);
            Unread = true;
        }

        public Guid Id { get; private set; }
        public NotificationKind Kind { get; private set; }
        public bool Unread { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public string Content { get; private set; }
        public string Method { get; private set; }
        public Guid UserId { get; private set; }
        public virtual User User { get; private set; }

        public void SetAsRead()
        {
            Unread = false;
        }
    }
}