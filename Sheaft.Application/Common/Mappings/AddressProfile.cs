using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Mappings
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
            CreateMap<BirthAddress, BirthAddressViewModel>();
            CreateMap<BirthAddressViewModel, BirthAddressInput>();

            CreateMap<UserAddress, AddressDto>();
            CreateMap<UserAddress, AddressViewModel>();

            CreateMap<AddressDto, AddressInput>();
            CreateMap<AddressViewModel, AddressInput>();
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
