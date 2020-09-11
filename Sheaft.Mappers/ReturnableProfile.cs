using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;
using Sheaft.Models.ViewModels;

namespace Sheaft.Mappers
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
