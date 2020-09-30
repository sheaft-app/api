using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
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
                .IncludeBase<Legal, BaseLegalDto>();

            CreateMap<CreateBusinessLegalInput, CreateBusinessLegalCommand>();
            CreateMap<UpdateBusinessLegalInput, UpdateBusinessLegalCommand>();
        }
    }
}
