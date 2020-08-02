using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class SimpleAddressProfile : Profile
    {
        public SimpleAddressProfile()
        {
            CreateMap<SimpleAddress, AddressDto>();
        }
    }
}
