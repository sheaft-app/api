using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Interop.Enums;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;
using Sheaft.Models.ViewModels;

namespace Sheaft.Mappers
{
    public class ConsumerProfile : Profile
    {
        public ConsumerProfile()
        {
            CreateMap<Consumer, UserViewModel>()
                .ForMember(c => c.Address, opt => opt.MapFrom(d => d.Address))
                .ForMember(c => c.BillingAddress, opt => opt.MapFrom(d => d.BillingAddress));

            CreateMap<Consumer, UserDto>()
                .ForMember(c => c.Address, opt => opt.MapFrom(d => d.Address))
                .ForMember(c => c.BillingAddress, opt => opt.MapFrom(d => d.BillingAddress));

            CreateMap<Consumer, ConsumerDto>()
                .IncludeBase<Consumer, UserDto>();
        }
    }
}
