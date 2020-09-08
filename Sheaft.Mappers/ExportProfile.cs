using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Models.Inputs;

namespace Sheaft.Mappers
{
    public class ExportProfile : Profile
    {
        public ExportProfile()
        {
            CreateMap<ExportPickingOrdersInput, QueueExportPickingOrderCommand>();
            CreateMap<IdInput, QueueExportUserDataCommand>();
        } 
    }
}
