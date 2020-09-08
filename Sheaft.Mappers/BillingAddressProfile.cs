using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class BillingAddressProfile : Profile
    {
        public BillingAddressProfile()
        {
            CreateMap<BillingAddress, AddressDto>();
        }
    }
}
