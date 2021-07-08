using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class AgreementViewProfile : Profile
    {
        public AgreementViewProfile()
        {
            CreateMap<Domain.Agreement, AgreementViewModel>();
            CreateMap<Domain.Agreement, ShortAgreementViewModel>();
        }
    }
}
