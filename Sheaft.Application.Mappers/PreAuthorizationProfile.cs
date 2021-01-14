using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Application.Models;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Mappers
{
    public class PreAuthorizationProfile : Profile
    {
        public PreAuthorizationProfile()
        {
            CreateMap<PreAuthorization, PreAuthorizationDto>();
            CreateMap<CreatePreAuthorizationInput, CreatePreAuthorizationCommand>();
        }
    }
}
