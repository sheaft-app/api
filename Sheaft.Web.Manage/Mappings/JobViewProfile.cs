using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class JobViewProfile : Profile
    {
        public JobViewProfile()
        {
            CreateMap<Domain.Job, JobViewModel>();
            CreateMap<JobDto, JobViewModel>();
            CreateMap<JobViewModel, JobDto>();
        }
    }
}
