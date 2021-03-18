using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Services.Consumer.Commands;

namespace Sheaft.Services.Mappings
{
    public class ConsumerInputProfile : Profile
    {
        public ConsumerInputProfile()
        {
            CreateMap<RegisterConsumerDto, RegisterConsumerCommand>();
            
            CreateMap<UpdateConsumerDto, UpdateConsumerCommand>()
                .ForMember(c => c.ConsumerId, opt => opt.MapFrom(r => r.Id));;
        }
    }
}
