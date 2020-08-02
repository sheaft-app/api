using System;
using Sheaft.Exceptions;
using Sheaft.Interop;
using Sheaft.Interop.Enums;

namespace Sheaft.Domain.Models
{
    public class Notification : IEntity
    {
        protected Notification()
        {
        }

        public Notification(Guid id, NotificationKind kind, string method, string content, User user):this(id, kind, method, content, user, user?.Company)
        {
        }

        public Notification(Guid id, NotificationKind kind, string method, string content, Company group):this(id, kind, method, content, null, group)
        {
        }

        private Notification(Guid id, NotificationKind kind, string method, string content, User user, Company group)
        {
            if (content == null)
                throw new ValidationException(MessageKind.Notification_Require_Content);

            Id = id;            
            Kind = kind;
            Method = method;
            User = user;
            Group = group;
            Content = content;
            Unread = true;
        }

        public Guid Id { get; private set; }
        public NotificationKind Kind { get; private set; }
        public bool Unread { get; set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Content { get; private set; }
        public string Method { get; set; }
        public virtual Company Group { get; set; }
        public virtual User User { get; set; }

        public void SetAsRead()
        {
            Unread = false;
        }
    }
}