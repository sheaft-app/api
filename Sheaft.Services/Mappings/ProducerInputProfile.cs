using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Services.Producer.Commands;

namespace Sheaft.Services.Mappings
{
    public class ProducerInputProfile : Profile
    {
        public ProducerInputProfile()
        {
            CreateMap<RegisterProducerDto, RegisterProducerCommand>();
            CreateMap<UpdateProducerDto, UpdateProducerCommand>()
                .ForMember(c => c.ProducerId, opt => opt.MapFrom(r => r.Id));
        }
    }
}
