using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;
using System.Linq;

namespace Sheaft.Application.Mappers
{
    public class ProducerProfile : Profile
    {
        public ProducerProfile()
        {
            CreateMap<Business, ProducerViewModel>()
                  .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<Producer, ProducerViewModel>()
                .IncludeBase<Business, ProducerViewModel>()
                  .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag.Id)));

            CreateMap<Producer, UserProfileDto>();

            CreateMap<Producer, UserDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<Producer, ProducerDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)));

            CreateMap<Producer, ProducerSummaryDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<RegisterProducerInput, RegisterProducerCommand>();
            CreateMap<UpdateProducerInput, UpdateProducerCommand>();
        }
    }
}
