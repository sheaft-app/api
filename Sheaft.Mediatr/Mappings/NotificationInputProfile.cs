using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Notification.Commands;

namespace Sheaft.Mediatr.Mappings
{
    public class NotificationInputProfile : Profile
    {
        public NotificationInputProfile()
        {
            CreateMap<ResourceIdDto, MarkUserNotificationAsReadCommand>()
                    .ForMember(c => c.NotificationId, opt => opt.MapFrom(r => r.Id));
        }
    }
}
