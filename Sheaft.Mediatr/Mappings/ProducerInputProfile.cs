using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.Mediatr.Mappings
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
