using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
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
            CreateMap<ConsumerInput, UpdateConsumerCommand>();
        }
    }
}
