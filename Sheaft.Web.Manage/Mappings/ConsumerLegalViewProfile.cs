using AutoMapper;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class ConsumerLegalViewProfile : Profile
    {
        public ConsumerLegalViewProfile()
        {
            CreateMap<ConsumerLegal, LegalViewModel>();
            CreateMap<ConsumerLegal, ConsumerLegalViewModel>();
        }
    }
}
