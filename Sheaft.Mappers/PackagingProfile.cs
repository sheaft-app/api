using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;
using Sheaft.Models.ViewModels;

namespace Sheaft.Mappers
{
    public class PackagingProfile : Profile
    {
        public PackagingProfile()
        {
            CreateMap<Packaging, PackagingDto>();
            CreateMap<Packaging, PackagingViewModel>();

            CreateMap<CreatePackagingInput, CreatePackagingCommand>();
            CreateMap<UpdatePackagingInput, UpdatePackagingCommand>();
            CreateMap<IdInput, DeletePackagingCommand>();
        }
    }
}
