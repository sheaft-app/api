using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class BusinessLegalViewProfile : Profile
    {
        public BusinessLegalViewProfile()
        {
            CreateMap<BusinessLegal, LegalViewModel>();
            CreateMap<BusinessLegal, BusinessLegalViewModel>();

            CreateMap<BusinessLegalDto, LegalViewModel>();
            CreateMap<BusinessLegalDto, BusinessLegalViewModel>();

            CreateMap<LegalViewModel, BusinessLegalDto>();
            CreateMap<BusinessLegalViewModel, BusinessLegalDto>();
        }
    }
}
