using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Notification.Commands;

namespace Sheaft.Mappings
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Domain.Notification, NotificationDto>();

            CreateMap<IdInput, MarkUserNotificationAsReadCommand>()
                    .ForMember(c => c.NotificationId, opt => opt.MapFrom(r => r.Id));
        }
    }
}
