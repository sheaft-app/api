using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.Dto
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public NotificationKind Kind { get; set; }
        public bool Unread { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public string Content { get; set; }
        public string Method { get; set; }
    }
}