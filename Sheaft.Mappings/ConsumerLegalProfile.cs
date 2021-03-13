using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.Legal.Commands;
using Sheaft.Domain;

namespace Sheaft.Mappings
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
                .IncludeBase<Domain.Legal, BaseLegalDto>();

            CreateMap<CreateConsumerLegalInput, CreateConsumerLegalCommand>();
            CreateMap<UpdateConsumerLegalInput, UpdateConsumerLegalCommand>()
                .ForMember(c => c.LegalId, opt => opt.MapFrom(r => r.Id));;
        }
    }
}
