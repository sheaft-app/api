using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.Ubo.Commands;

namespace Sheaft.Mappings
{
    public class UboProfile : Profile
    {
        public UboProfile()
        {
            CreateMap<Domain.Ubo, UboDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                .ForMember(d => d.BirthPlace, opt => opt.MapFrom(r => r.BirthPlace));

            CreateMap<Domain.Ubo, UboViewModel>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                .ForMember(d => d.BirthPlace, opt => opt.MapFrom(r => r.BirthPlace));

            CreateMap<CreateUboInput, CreateUboCommand>();
            CreateMap<UpdateUboInput, UpdateUboCommand>()
                .ForMember(c => c.UboId, opt => opt.MapFrom(r => r.Id));;
            CreateMap<IdInput, DeleteUboCommand>()
                    .ForMember(c => c.UboId, opt => opt.MapFrom(r => r.Id));
        }
    }
}
