using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Mappings
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<DeliveryAddress, AddressDto>();
            CreateMap<ExpectedAddress, AddressDto>();
            CreateMap<BankAddress, AddressDto>();
            CreateMap<OwnerAddress, AddressDto>();            
            CreateMap<LegalAddress, AddressDto>();
            CreateMap<UboAddress, AddressDto>();
            CreateMap<UserAddress, AddressDto>();
            CreateMap<BirthAddress, BirthAddressDto>();
        }
    }
}
