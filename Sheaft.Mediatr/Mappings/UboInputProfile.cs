using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Ubo.Commands;

namespace Sheaft.Mediatr.Mappings
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
