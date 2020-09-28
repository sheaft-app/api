using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class UboProfile : Profile
    {
        public UboProfile()
        {
            CreateMap<Ubo, UboDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                .ForMember(d => d.BirthPlace, opt => opt.MapFrom(r => r.BirthPlace));

            CreateMap<Ubo, UboViewModel>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                .ForMember(d => d.BirthPlace, opt => opt.MapFrom(r => r.BirthPlace));

            CreateMap<CreateUboInput, CreateUboCommand>();
            CreateMap<UpdateUboInput, UpdateUboCommand>();
            CreateMap<IdInput, RemoveUboCommand>();
        }
    }
}
