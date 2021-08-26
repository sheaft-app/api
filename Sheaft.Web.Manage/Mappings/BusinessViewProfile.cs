using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class BusinessViewProfile : Profile
    {
        public BusinessViewProfile()
        {
            CreateMap<Domain.Business, UserViewModel>();
            CreateMap<Domain.Business, ProducerViewModel>()
                .IncludeBase<Domain.Business, UserViewModel>();
            
            CreateMap<Domain.Business, StoreViewModel>()
                .IncludeBase<Domain.Business, UserViewModel>();
        }
    }
}
