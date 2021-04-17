using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.PreAuthorization;
using Sheaft.Mediatr.PreAuthorization.Commands;

namespace Sheaft.Mediatr.Mappings
{
    public class PreAuthorizationInputProfile : Profile
    {
        public PreAuthorizationInputProfile()
        {
            CreateMap<CreatePreAuthorizationDto, CreatePreAuthorizationForOrderCommand>();
        }
    }
}