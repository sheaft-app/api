using System.Linq;
using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class StoreViewProfile : Profile
    {
        public StoreViewProfile()
        {
            CreateMap<Domain.Store, UserViewModel>();
            CreateMap<Domain.Store, StoreViewModel>()
                .IncludeBase<Store, UserViewModel>()
                  .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag.Id)));

            CreateMap<StoreDto, StoreViewModel>();
            CreateMap<StoreDto, UserViewModel>();
            CreateMap<StoreViewModel, StoreDto>();
            CreateMap<StoreViewModel, UserDto>();
        }
    }
}
