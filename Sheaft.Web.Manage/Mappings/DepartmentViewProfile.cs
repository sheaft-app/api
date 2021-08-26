using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class DepartmentViewProfile : Profile
    {
        public DepartmentViewProfile()
        {
            CreateMap<Domain.Department, DepartmentViewModel>();
        }
    }
}
