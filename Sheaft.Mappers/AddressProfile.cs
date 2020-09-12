using AutoMapper;
using Sheaft.Application.Commands;
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
            CreateMap<BillingAddress, AddressDto>();
            CreateMap<BillingAddress, AddressViewModel>();
            CreateMap<LegalAddress, AddressDto>();
            CreateMap<LegalAddress, AddressViewModel>();

            CreateMap<AddressDto, AddressInput>();
            CreateMap<AddressViewModel, AddressInput>();
        }
    }
}
