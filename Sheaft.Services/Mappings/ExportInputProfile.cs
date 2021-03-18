using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Services.PickingOrders.Commands;
using Sheaft.Services.User.Commands;

namespace Sheaft.Services.Mappings
{
    public class ExportInputProfile : Profile
    {
        public ExportInputProfile()
        {
            CreateMap<ExportPickingOrdersDto, QueueExportPickingOrderCommand>();
            CreateMap<ResourceIdDto, QueueExportUserDataCommand>()
                    .ForMember(c => c.UserId, opt => opt.MapFrom(r => r.Id));
        } 
    }
}
