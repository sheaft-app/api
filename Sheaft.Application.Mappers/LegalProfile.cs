using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class LegalProfile : Profile
    {
        public LegalProfile()
        {
            CreateMap<Legal, BaseLegalDto>();
            CreateMap<Legal, LegalViewModel>();
            CreateMap<BusinessLegal, LegalViewModel>();
            CreateMap<ConsumerLegal, LegalViewModel>();

            CreateMap<BusinessLegal, BusinessLegalViewModel>()
                .ForMember(c => c.Address, opt => opt.MapFrom(e => e.Address))
                .ForMember(c => c.UboDeclaration, opt => opt.MapFrom(e => e.UboDeclaration));

            CreateMap<BusinessLegal, BusinessLegalDto>()
                .IncludeBase<Legal, BaseLegalDto>();

            CreateMap<ConsumerLegal, ConsumerLegalViewModel>()
                .ForMember(c => c.Owner, opt => opt.MapFrom(e => e.Owner));

            CreateMap<ConsumerLegal, ConsumerLegalDto>()
                .IncludeBase<Legal, BaseLegalDto>();

            CreateMap<CreateBusinessLegalInput, CreateBusinessLegalCommand>();
            CreateMap<CreateConsumerLegalInput, CreateConsumerLegalCommand>();
            CreateMap<UpdateBusinessLegalInput, UpdateBusinessLegalCommand>();
            CreateMap<UpdateConsumerLegalInput, UpdateConsumerLegalCommand>();

            CreateMap<CreateUboInput, CreateUboCommand>();
            CreateMap<UpdateUboInput, UpdateUboCommand>();
            CreateMap<IdInput, RemoveUboCommand>();
        }
    }
}
