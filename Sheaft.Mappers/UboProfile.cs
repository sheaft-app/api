using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class UboProfile : Profile
    {
        public UboProfile()
        {
            CreateMap<Ubo, UboDto>();
        }
    }
}
