using System.Linq;
using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            
            CreateMap<User, ProducerDto>()
                .IncludeBase<User, UserDto>();
            CreateMap<User, StoreDto>()
                .IncludeBase<User, UserDto>();
            CreateMap<User, ConsumerDto>()
                .IncludeBase<User, UserDto>();

            CreateMap<PurchaseOrderSender, UserDto>()
                .ForMember(p => p.Address, opt => opt.MapFrom(a => GetAddressFromString(a.Address)));
            CreateMap<PurchaseOrderSender, ConsumerDto>()
                .ForMember(p => p.Address, opt => opt.MapFrom(a => GetAddressFromString(a.Address)));
            CreateMap<PurchaseOrderSender, StoreDto>()
                .ForMember(p => p.Address, opt => opt.MapFrom(a => GetAddressFromString(a.Address)));
            
            CreateMap<PurchaseOrderVendor, UserDto>()
                .ForMember(p => p.Address, opt => opt.MapFrom(a => GetAddressFromString(a.Address)));
            CreateMap<PurchaseOrderVendor, ProducerDto>()
                .ForMember(p => p.Address, opt => opt.MapFrom(a => GetAddressFromString(a.Address)));
            
            CreateMap<UserDto, ConsumerDto>();
            CreateMap<UserDto, ProducerDto>();
            CreateMap<UserDto, StoreDto>();
            CreateMap<ConsumerDto, UserDto>();
            CreateMap<ProducerDto, UserDto>();
            CreateMap<StoreDto, UserDto>();
        }

        private static AddressDto GetAddressFromString(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return new AddressDto();
            
            var splitted = address.Split(',');
            if (splitted.Length < 2)
                return new AddressDto();
            
            var city = splitted.Last();
            var addressSplitted = splitted[splitted.Length - 2].Split('\n');
            var zipcode = addressSplitted.Last();
            var line1 = addressSplitted.First();
            var line2 = addressSplitted.Length > 2 ? addressSplitted[1] : null;

            return new AddressDto
            {
                Line1 = line1,
                Line2 = line2,
                Zipcode = zipcode,
                City = city
            };
        }
    }
}
