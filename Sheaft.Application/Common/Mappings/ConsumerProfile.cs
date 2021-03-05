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
                .ForMember(c => c.Summary, opt => opt.MapFrom(e => e.ProfileInformation.Summary))
                .ForMember(c => c.Description, opt => opt.MapFrom(e => e.ProfileInformation.Description))
                .ForMember(c => c.Facebook, opt => opt.MapFrom(e => e.ProfileInformation.Facebook))
                .ForMember(c => c.Instagram, opt => opt.MapFrom(e => e.ProfileInformation.Instagram))
                .ForMember(c => c.Twitter, opt => opt.MapFrom(e => e.ProfileInformation.Twitter))
                .ForMember(c => c.Website, opt => opt.MapFrom(e => e.ProfileInformation.Website))
                .ForMember(c => c.Pictures, opt => opt.MapFrom(e => e.ProfileInformation.Pictures));

            CreateMap<Domain.Consumer, ConsumerDto>()
                .IncludeBase<Domain.Consumer, UserDto>();

            CreateMap<RegisterConsumerInput, RegisterConsumerCommand>()
                .ForMember(c => c.ConsumerId, opt => opt.MapFrom(r => r.Id));;
            
            CreateMap<UpdateConsumerInput, UpdateConsumerCommand>()
                .ForMember(c => c.ConsumerId, opt => opt.MapFrom(r => r.Id));;
        }
    }
}
