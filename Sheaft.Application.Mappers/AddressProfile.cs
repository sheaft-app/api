using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<DeliveryAddress, AddressDto>();
            CreateMap<DeliveryAddress, AddressViewModel>();
            CreateMap<ExpectedAddress, AddressDto>();
            CreateMap<ExpectedAddress, AddressViewModel>();

            CreateMap<BankAddress, AddressDto>();
            CreateMap<BankAddress, AddressViewModel>();
            CreateMap<OwnerAddress, AddressDto>();
            CreateMap<OwnerAddress, AddressViewModel>();
            CreateMap<LegalAddress, AddressDto>();
            CreateMap<LegalAddress, AddressViewModel>();
            CreateMap<UboAddress, AddressDto>();
            CreateMap<UboAddress, AddressViewModel>();

            CreateMap<BirthAddress, AddressDto>();
            CreateMap<BirthAddress, AddressViewModel>();

            CreateMap<UserAddress, AddressDto>();
            CreateMap<UserAddress, AddressViewModel>();

            CreateMap<AddressDto, AddressInput>();
            CreateMap<AddressInput, AddressDto>();
            CreateMap<AddressDto, FullAddressInput>();
            CreateMap<FullAddressInput, AddressDto>();
            CreateMap<AddressDto, LocationAddressInput>();
            CreateMap<LocationAddressInput, AddressDto>();

            CreateMap<AddressViewModel, AddressInput>();
            CreateMap<AddressInput, AddressViewModel>();
            CreateMap<AddressViewModel, FullAddressInput>();
            CreateMap<FullAddressInput, AddressViewModel>();
            CreateMap<AddressViewModel, LocationAddressInput>();
            CreateMap<LocationAddressInput, AddressViewModel>();
        }
    }
}
