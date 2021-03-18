using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class OwnerViewProfile : Profile
    {
        public OwnerViewProfile()
        {
            CreateMap<Owner, OwnerViewModel>();
            CreateMap<OwnerDto, OwnerViewModel>();
            CreateMap<OwnerViewModel, OwnerDto>();
        }
    }
}
