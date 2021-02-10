using System.Linq;
using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.Producer.Commands;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Mappings
{
    public class ProducerProfile : Profile
    {
        public ProducerProfile()
        {
            CreateMap<Business, ProducerViewModel>()
                  .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<Domain.Producer, ProducerViewModel>()
                .IncludeBase<Business, ProducerViewModel>()
                  .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag.Id)));

            CreateMap<Domain.Producer, UserProfileDto>();

            CreateMap<Domain.Producer, UserDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<Domain.Producer, ProducerDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)));

            CreateMap<Domain.Producer, ProducerSummaryDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<RegisterProducerInput, RegisterProducerCommand>();
            CreateMap<UpdateProducerInput, UpdateProducerCommand>();
        }
    }
}
