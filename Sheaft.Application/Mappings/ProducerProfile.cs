using System.Linq;
using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Mappings
{
    public class ProducerProfile : Profile
    {
        public ProducerProfile()
        {
            CreateMap<Domain.Producer, UserDto>();
            
            CreateMap<Domain.Producer, ProducerDto>()
                .IncludeBase<Producer, UserDto>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)));
        }
    }
}