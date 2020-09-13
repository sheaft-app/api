using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;
using Sheaft.Models.ViewModels;

namespace Sheaft.Mappers
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<UserAddress, AddressDto>();
            CreateMap<UserAddress, AddressViewModel>();

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

            CreateMap<AddressDto, AddressInput>();
            CreateMap<AddressViewModel, AddressInput>();
        }
    }
}
