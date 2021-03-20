using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Consumer.Commands;

namespace Sheaft.Mediatr.Mappings
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
