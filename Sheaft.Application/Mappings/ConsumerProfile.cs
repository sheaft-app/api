using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class ConsumerProfile : Profile
    {
        public ConsumerProfile()
        {
            CreateMap<Domain.Consumer, UserDto>();
            CreateMap<Domain.Consumer, ConsumerDto>()
                .IncludeBase<Domain.Consumer, UserDto>();
        }
    }
}
