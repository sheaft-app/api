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
                .ForMember(c => c.DepartmentId, opt => opt.MapFrom(d => d.Address.Department.Id));

            CreateMap<Consumer, UserDto>()
                .ForMember(c => c.Address, opt => opt.MapFrom(d => d.Address));

            CreateMap<Consumer, ConsumerDto>()
                .IncludeBase<Consumer, UserDto>();

            CreateMap<RegisterConsumerInput, RegisterConsumerCommand>();
            CreateMap<UpdateConsumerInput, UpdateConsumerCommand>();
            CreateMap<CreateConsumerLegalInput, CreateConsumerLegalCommand>();
        }
    }
}
