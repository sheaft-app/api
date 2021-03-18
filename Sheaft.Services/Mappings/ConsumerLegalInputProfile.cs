using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Services.Legal.Commands;

namespace Sheaft.Services.Mappings
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
