using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Job.Commands;

namespace Sheaft.Mediatr.Mappings
{
    public class JobInputProfile : Profile
    {
        public JobInputProfile()
        {
            CreateMap<ResourceIdsDto, ResumeJobsCommand>()
                .ForMember(c => c.JobIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<ResourceIdsDto, PauseJobsCommand>()
                .ForMember(c => c.JobIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<ResourceIdsDto, RetryJobsCommand>()
                .ForMember(c => c.JobIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<ResourceIdsDto, ArchiveJobsCommand>()
                .ForMember(c => c.JobIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<ResourceIdsWithReasonDto, CancelJobsCommand>()
                .ForMember(c => c.JobIds, opt => opt.MapFrom(r => r.Ids));
        }
    }
}
