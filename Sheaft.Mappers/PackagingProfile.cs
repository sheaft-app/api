using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;

namespace Sheaft.Mappers
{
    public class PackagingProfile : Profile
    {
        public PackagingProfile()
        {
            CreateMap<Packaging, PackagingDto>();

            CreateMap<CreatePackagingInput, CreatePackagingCommand>();
            CreateMap<UpdatePackagingInput, UpdatePackagingCommand>();
            CreateMap<IdInput, DeletePackagingCommand>();
        }
    }
}
