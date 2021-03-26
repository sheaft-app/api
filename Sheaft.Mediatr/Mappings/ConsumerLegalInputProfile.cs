using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Legal.Commands;

namespace Sheaft.Mediatr.Mappings
{
    public class ConsumerLegalInputProfile : Profile
    {
        public ConsumerLegalInputProfile()
        {
            CreateMap<CreateConsumerLegalDto, CreateConsumerLegalCommand>();
            CreateMap<UpdateConsumerLegalDto, UpdateConsumerLegalCommand>()
                .ForMember(c => c.LegalId, opt => opt.MapFrom(r => r.Id));;
        }
    }
}
