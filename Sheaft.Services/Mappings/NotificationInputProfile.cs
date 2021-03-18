using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Services.Notification.Commands;

namespace Sheaft.Services.Mappings
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
