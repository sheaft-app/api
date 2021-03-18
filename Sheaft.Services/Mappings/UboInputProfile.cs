using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Services.Ubo.Commands;

namespace Sheaft.Services.Mappings
{
    public class UboInputProfile : Profile
    {
        public UboInputProfile()
        {
            CreateMap<CreateUboDto, CreateUboCommand>();
            CreateMap<UpdateUboDto, UpdateUboCommand>()
                .ForMember(c => c.UboId, opt => opt.MapFrom(r => r.Id));;
            CreateMap<ResourceIdDto, DeleteUboCommand>()
                    .ForMember(c => c.UboId, opt => opt.MapFrom(r => r.Id));
        }
    }
}
