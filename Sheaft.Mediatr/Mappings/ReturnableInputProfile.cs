using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Returnable.Commands;

namespace Sheaft.Mediatr.Mappings
{
    public class ReturnableInputProfile : Profile
    {
        public ReturnableInputProfile()
        {
            CreateMap<CreateReturnableDto, CreateReturnableCommand>();
            CreateMap<UpdateReturnableDto, UpdateReturnableCommand>()
                .ForMember(c => c.ReturnableId, opt => opt.MapFrom(r => r.Id));;
            CreateMap<ResourceIdDto, DeleteReturnableCommand>()
                .ForMember(c => c.ReturnableId, opt => opt.MapFrom(r => r.Id));
        }
    }
}
