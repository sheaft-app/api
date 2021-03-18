using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class ReturnableProfile : Profile
    {
        public ReturnableProfile()
        {
            CreateMap<Domain.Returnable, ReturnableDto>();
        }
    }
}
