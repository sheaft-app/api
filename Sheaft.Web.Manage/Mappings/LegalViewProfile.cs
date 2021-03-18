using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class LegalViewProfile : Profile
    {
        public LegalViewProfile()
        {
            CreateMap<Domain.Legal, LegalViewModel>();
        }
    }
}
