using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Agreement.Commands;

namespace Sheaft.Mediatr.Mappings
{
    public class AgreementInputProfile : Profile
    {
        public AgreementInputProfile()
        {
            CreateMap<AssignCatalogToAgreementDto, AssignCatalogToAgreementCommand>();
            CreateMap<CreateAgreementDto, CreateAgreementCommand>();
            CreateMap<AcceptAgreementDto, AcceptAgreementCommand>()
                .ForMember(c => c.AgreementId, opt => opt.MapFrom(r => r.Id));
            CreateMap<ResourceIdsWithReasonDto, CancelAgreementsCommand>()
                .ForMember(c => c.AgreementIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<ResourceIdsWithReasonDto, RefuseAgreementsCommand>()
                .ForMember(c => c.AgreementIds, opt => opt.MapFrom(r => r.Ids));
        }
    }
}
