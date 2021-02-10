using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.Consumer.Commands;

namespace Sheaft.Application.Common.Mappings
{
    public class ConsumerProfile : Profile
    {
        public ConsumerProfile()
        {
            CreateMap<Domain.Consumer, UserViewModel>();
            CreateMap<Domain.Consumer, ConsumerViewModel>();

            CreateMap<Domain.Consumer, UserDto>()
                .ForMember(c => c.Address, opt => opt.MapFrom(d => d.Address));

            CreateMap<Domain.Consumer, ConsumerDto>()
                .IncludeBase<Domain.Consumer, UserDto>();

            CreateMap<RegisterConsumerInput, RegisterConsumerCommand>()
                .ForMember(c => c.ConsumerId, opt => opt.MapFrom(r => r.Id));;
            CreateMap<UpdateConsumerInput, UpdateConsumerCommand>()
                .ForMember(c => c.ConsumerId, opt => opt.MapFrom(r => r.Id));;
        }
    }
}
