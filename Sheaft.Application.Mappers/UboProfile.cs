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
            CreateMap<Ubo, UboDto>();

            CreateMap<CreateUboInput, CreateUboCommand>();
            CreateMap<UpdateUboInput, UpdateUboCommand>();
            CreateMap<IdInput, RemoveUboCommand>();
        }
    }
}
