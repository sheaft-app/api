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
            CreateMap<FullAddress, AddressDto>();
            CreateMap<FullAddress, AddressViewModel>();

            CreateMap<LocationAddress, AddressDto>();
            CreateMap<LocationAddress, AddressViewModel>();
            CreateMap<DeliveryAddress, AddressDto>();
            CreateMap<DeliveryAddress, AddressViewModel>();

            CreateMap<Address, AddressDto>();
            CreateMap<Address, AddressViewModel>();
            CreateMap<BillingAddress, AddressDto>();
            CreateMap<BillingAddress, AddressViewModel>();
            CreateMap<LegalAddress, AddressDto>();
            CreateMap<LegalAddress, AddressViewModel>();

            CreateMap<AddressDto, AddressInput>();
            CreateMap<AddressViewModel, AddressInput>();
        }
    }
}
