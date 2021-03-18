using AutoMapper;
using Sheaft.Application.Models;
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
            
            CreateMap<ConsumerLegalDto, ConsumerLegalViewModel>();
            CreateMap<ConsumerLegalDto, LegalViewModel>();
            
            CreateMap<LegalViewModel, ConsumerLegalDto>();
            CreateMap<ConsumerLegalViewModel, ConsumerLegalDto>();
        }
    }
}
