using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.PickingOrders.Commands;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.Mediatr.Mappings
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
