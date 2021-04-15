using AutoMapper;
using Sheaft.Application.Models;
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

            CreateMap<PurchaseOrderSender, UserDto>();
            CreateMap<PurchaseOrderSender, ConsumerDto>();
            CreateMap<PurchaseOrderSender, StoreDto>();
            
            CreateMap<PurchaseOrderVendor, UserDto>();
            CreateMap<PurchaseOrderVendor, ProducerDto>();

            CreateMap<PurchaseOrderSender, UserProfileViewModel>();
            CreateMap<PurchaseOrderSender, UserViewModel>();
            
            CreateMap<PurchaseOrderVendor, UserProfileViewModel>();
            CreateMap<PurchaseOrderVendor, UserViewModel>();
            
            CreateMap<UserDto, ConsumerDto>();
            CreateMap<UserDto, ProducerDto>();
            CreateMap<UserDto, StoreDto>();
            CreateMap<ConsumerDto, UserDto>();
            CreateMap<ProducerDto, UserDto>();
            CreateMap<StoreDto, UserDto>();
            
            CreateMap<UserDto, UserProfileViewModel>();
            CreateMap<UserDto, UserViewModel>();
            
            CreateMap<UserViewModel, UserDto>();
            CreateMap<UserViewModel, ProducerDto>();
            CreateMap<UserViewModel, StoreDto>();
            CreateMap<UserViewModel, ConsumerDto>();
            
            CreateMap<UserProfileViewModel, UserDto>();
            CreateMap<UserProfileViewModel, ProducerDto>();
            CreateMap<UserProfileViewModel, StoreDto>();
            CreateMap<UserProfileViewModel, ConsumerDto>();
        }
    }
}
