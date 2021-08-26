using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class ConsumerViewProfile : Profile
    {
        public ConsumerViewProfile()
        {
            CreateMap<Domain.Consumer, UserViewModel>();
            CreateMap<Domain.Consumer, ConsumerViewModel>()
                .IncludeBase<Domain.Consumer, UserViewModel>();
        }
    }
}
