using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.Job.Commands;

namespace Sheaft.Application.Common.Mappings
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<Domain.Job, JobDto>()
                .ForMember(d => d.User, opt => opt.MapFrom(r => r.User));

            CreateMap<Domain.Job, JobViewModel>()
                .ForMember(d => d.User, opt => opt.MapFrom(r => r.User));

            CreateMap<IdsInput, ResumeJobsCommand>();
            CreateMap<IdsInput, PauseJobsCommand>();
            CreateMap<IdsInput, RetryJobsCommand>();
            CreateMap<IdsInput, ArchiveJobsCommand>();
            CreateMap<IdsWithReasonInput, CancelJobsCommand>();
        }
    }
}
