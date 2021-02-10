using AutoMapper;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.PickingOrders.Commands;
using Sheaft.Application.User.Commands;

namespace Sheaft.Application.Common.Mappings
{
    public class ExportProfile : Profile
    {
        public ExportProfile()
        {
            CreateMap<ExportPickingOrdersInput, QueueExportPickingOrderCommand>();
            CreateMap<IdInput, QueueExportUserDataCommand>()
                    .ForMember(c => c.UserId, opt => opt.MapFrom(r => r.Id));
        } 
    }
}
