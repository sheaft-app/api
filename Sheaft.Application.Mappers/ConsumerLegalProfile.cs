using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class ConsumerLegalProfile : Profile
    {
        public ConsumerLegalProfile()
        {
            CreateMap<ConsumerLegal, LegalViewModel>()
                .ForMember(c => c.Owner, opt => opt.MapFrom(e => e.Owner))
                .ForMember(c => c.Documents, opt => opt.MapFrom(e => e.Documents));

            CreateMap<ConsumerLegal, ConsumerLegalViewModel>()
                .IncludeBase<ConsumerLegal, LegalViewModel>();

            CreateMap<ConsumerLegal, ConsumerLegalDto>()
                .IncludeBase<Legal, BaseLegalDto>();

            CreateMap<CreateConsumerLegalInput, CreateConsumerLegalCommand>();
            CreateMap<UpdateConsumerLegalInput, UpdateConsumerLegalCommand>();
        }
    }
}
