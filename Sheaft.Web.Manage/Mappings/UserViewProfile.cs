using AutoMapper;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class UserViewProfile : Profile
    {
        public UserViewProfile()
        {
            CreateMap<Domain.User, UserViewModel>();
            CreateMap<Domain.User, UserProfileViewModel>();
            
            CreateMap<User, ProducerViewModel>()
                .IncludeBase<User, UserViewModel>();
            CreateMap<User, StoreViewModel>()
                .IncludeBase<User, UserViewModel>();
            CreateMap<User, ConsumerViewModel>()
                .IncludeBase<User, UserViewModel>();

            CreateMap<PurchaseOrderSender, UserProfileViewModel>();
            CreateMap<PurchaseOrderSender, UserViewModel>();
            
            CreateMap<PurchaseOrderVendor, UserProfileViewModel>();
            CreateMap<PurchaseOrderVendor, UserViewModel>();
        }
    }
}
