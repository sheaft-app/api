using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.Legal.Commands;
using Sheaft.Domain;

namespace Sheaft.Mappings
{
    public class BusinessLegalProfile : Profile
    {
        public BusinessLegalProfile()
        {
            CreateMap<BusinessLegal, LegalViewModel>()
                .ForMember(c => c.Owner, opt => opt.MapFrom(e => e.Owner))
                .ForMember(c => c.Documents, opt => opt.MapFrom(e => e.Documents));

            CreateMap<BusinessLegal, BusinessLegalViewModel>()
                .IncludeBase<BusinessLegal, LegalViewModel>()
                .ForMember(c => c.Address, opt => opt.MapFrom(e => e.Address))
                .ForMember(c => c.Declaration, opt => opt.MapFrom(e => e.Declaration));

            CreateMap<BusinessLegal, BusinessLegalDto>()
                .IncludeBase<Domain.Legal, BaseLegalDto>();

            CreateMap<CreateBusinessLegalInput, CreateBusinessLegalCommand>();
            CreateMap<UpdateBusinessLegalInput, UpdateBusinessLegalCommand>()
                .ForMember(c => c.LegalId, opt => opt.MapFrom(r => r.Id));;
        }
    }
}
