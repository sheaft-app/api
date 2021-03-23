using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class UserSettingViewProfile : Profile
    {
        public UserSettingViewProfile()
        {
            CreateMap<Domain.UserSetting, UserSettingViewModel>()
                .ForMember(u => u.Id, opt => opt.MapFrom(s => s.Setting.Id))
                .ForMember(u => u.Kind, opt => opt.MapFrom(s => s.Setting.Kind))
                .ForMember(u => u.Name, opt => opt.MapFrom(s => s.Setting.Name));
        }
    }
}