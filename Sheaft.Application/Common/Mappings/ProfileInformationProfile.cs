using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.User.Commands;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Mappings
{
    public class ProfileInformationProfile : Profile
    {
        public ProfileInformationProfile()
        {
            CreateMap<ProfileInformation, ProfileInformationDto>();
            
            CreateMap<UpdateUserProfileInput, UpdateUserProfileCommand>()
                .ForMember(c => c.UserId, opt => opt.MapFrom(r => r.Id));
            
            CreateMap<AddPictureToUserProfileInput, AddPictureToUserProfileCommand>()
                .ForMember(c => c.UserId, opt => opt.MapFrom(r => r.Id));

            CreateMap<IdInput, RemoveUserProfilePictureCommand>();
            CreateMap<IdsInput, RemoveUserProfilePicturesCommand>();
        }
    }
}