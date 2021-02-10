using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
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
