using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.Returnable.Commands;

namespace Sheaft.Mappings
{
    public class ReturnableProfile : Profile
    {
        public ReturnableProfile()
        {
            CreateMap<Domain.Returnable, ReturnableDto>();
            CreateMap<Domain.Returnable, ReturnableViewModel>();

            CreateMap<CreateReturnableInput, CreateReturnableCommand>();
            CreateMap<UpdateReturnableInput, UpdateReturnableCommand>()
                .ForMember(c => c.ReturnableId, opt => opt.MapFrom(r => r.Id));;
            CreateMap<IdInput, DeleteReturnableCommand>()
                .ForMember(c => c.ReturnableId, opt => opt.MapFrom(r => r.Id));
        }
    }
}
