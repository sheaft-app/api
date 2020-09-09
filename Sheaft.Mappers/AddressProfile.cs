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
            CreateMap<Address, AddressDto>();
            CreateMap<Address, AddressViewModel>();
            CreateMap<SimpleAddress, AddressDto>();
            CreateMap<SimpleAddress, AddressViewModel>();
            CreateMap<BillingAddress, AddressDto>();
            CreateMap<BillingAddress, AddressViewModel>();
            CreateMap<LegalAddress, AddressDto>();
            CreateMap<LegalAddress, AddressViewModel>();

            CreateMap<AddressDto, AddressInput>();
            CreateMap<AddressViewModel, AddressInput>();
        }
    }
}
