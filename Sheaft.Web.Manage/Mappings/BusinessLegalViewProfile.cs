using AutoMapper;
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
        }
    }
}
