using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(c => c.Summary, opt => opt.MapFrom(e => e.ProfileInformation.Summary))
                .ForMember(c => c.Description, opt => opt.MapFrom(e => e.ProfileInformation.Description))
                .ForMember(c => c.Facebook, opt => opt.MapFrom(e => e.ProfileInformation.Facebook))
                .ForMember(c => c.Instagram, opt => opt.MapFrom(e => e.ProfileInformation.Instagram))
                .ForMember(c => c.Twitter, opt => opt.MapFrom(e => e.ProfileInformation.Twitter))
                .ForMember(c => c.Website, opt => opt.MapFrom(e => e.ProfileInformation.Website))
                .ForMember(c => c.Pictures, opt => opt.MapFrom(e => e.ProfileInformation.Pictures));
            
            CreateMap<User, ProducerDto>()
                .IncludeBase<User, UserDto>();
            CreateMap<User, StoreDto>()
                .IncludeBase<User, UserDto>();
            CreateMap<User, ConsumerDto>()
                .IncludeBase<User, UserDto>();

            CreateMap<PurchaseOrderSender, UserDto>();
            CreateMap<PurchaseOrderSender, ConsumerDto>();
            CreateMap<PurchaseOrderSender, StoreDto>();
            
            CreateMap<PurchaseOrderVendor, UserDto>();
            CreateMap<PurchaseOrderVendor, ProducerDto>();
            
            CreateMap<UserDto, ConsumerDto>();
            CreateMap<UserDto, ProducerDto>();
            CreateMap<UserDto, StoreDto>();
            CreateMap<ConsumerDto, UserDto>();
            CreateMap<ProducerDto, UserDto>();
            CreateMap<StoreDto, UserDto>();
        }
    }
}
