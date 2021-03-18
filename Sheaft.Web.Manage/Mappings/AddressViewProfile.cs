using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class AddressViewProfile : Profile
    {
        public AddressViewProfile()
        {
            CreateMap<DeliveryAddress, AddressViewModel>();
            CreateMap<ExpectedAddress, AddressViewModel>();
            CreateMap<BankAddress, AddressViewModel>();
            CreateMap<OwnerAddress, AddressViewModel>();
            CreateMap<LegalAddress, AddressViewModel>();
            CreateMap<UboAddress, AddressViewModel>();
            CreateMap<UserAddress, AddressViewModel>();

            CreateMap<AddressViewModel, AddressDto>();
            CreateMap<AddressDto, AddressViewModel>();
            
            CreateMap<BirthAddress, BirthAddressViewModel>();
            CreateMap<BirthAddressViewModel, BirthAddressDto>();
            CreateMap<BirthAddressViewModel, AddressDto>();
        }
    }
}
