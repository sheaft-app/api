using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class LevelViewProfile : Profile
    {
        public LevelViewProfile()
        {
            CreateMap<Domain.Level, LevelViewModel>();
        }
    }
}
