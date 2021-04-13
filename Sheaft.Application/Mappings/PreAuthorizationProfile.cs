using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Mappings
{
    public class PreAuthorizationProfile : Profile
    {
        public PreAuthorizationProfile()
        {
            CreateMap<PreAuthorization, PreAuthorizationDto>();
        }
    }
}