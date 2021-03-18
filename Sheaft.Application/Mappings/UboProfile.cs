using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class UboProfile : Profile
    {
        public UboProfile()
        {
            CreateMap<Domain.Ubo, UboDto>();
        }
    }
}
