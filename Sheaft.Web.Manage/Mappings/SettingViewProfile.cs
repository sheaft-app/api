using System;
using AutoMapper;
using Sheaft.Mediatr.Setting.Commands;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class SettingViewProfile : Profile
    {
        public SettingViewProfile()
        {
            CreateMap<Domain.Setting, SettingViewModel>();

            CreateMap<SettingViewModel, CreateSettingCommand>();
            CreateMap<SettingViewModel, UpdateSettingCommand>()
                .ForMember(s => s.SettingId, opt => opt.MapFrom(e => e.Id));
            CreateMap<Guid, DeleteSettingCommand>()
                .ForMember(s => s.SettingId, opt => opt.MapFrom(e => e));
        }
    }
}