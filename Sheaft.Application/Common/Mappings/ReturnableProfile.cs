using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class ReturnableProfile : Profile
    {
        public ReturnableProfile()
        {
            CreateMap<Returnable, ReturnableDto>();
            CreateMap<Returnable, ReturnableViewModel>();

            CreateMap<CreateReturnableInput, CreateReturnableCommand>();
            CreateMap<UpdateReturnableInput, UpdateReturnableCommand>();
            CreateMap<IdInput, DeleteReturnableCommand>();
        }
    }
}
