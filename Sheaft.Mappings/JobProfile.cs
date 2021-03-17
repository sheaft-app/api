using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.Job.Commands;

namespace Sheaft.Mappings
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<Domain.Job, JobDto>()
                .ForMember(d => d.User, opt => opt.MapFrom(r => r.User));

            CreateMap<Domain.Job, JobViewModel>()
                .ForMember(d => d.User, opt => opt.MapFrom(r => r.User));

            CreateMap<IdsInput, ResumeJobsCommand>()
                .ForMember(c => c.JobIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<IdsInput, PauseJobsCommand>()
                .ForMember(c => c.JobIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<IdsInput, RetryJobsCommand>()
                .ForMember(c => c.JobIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<IdsInput, ArchiveJobsCommand>()
                .ForMember(c => c.JobIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<IdsWithReasonInput, CancelJobsCommand>()
                .ForMember(c => c.JobIds, opt => opt.MapFrom(r => r.Ids));
        }
    }
}
