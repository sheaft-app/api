using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;

namespace Sheaft.Mappers
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<Job, JobDto>()
                .ForMember(d => d.User, opt => opt.MapFrom(r => r.User));

            CreateMap<IdsInput, ResumeJobsCommand>();
            CreateMap<IdsInput, PauseJobsCommand>();
            CreateMap<IdsInput, RetryJobsCommand>();
            CreateMap<IdsInput, ArchiveJobsCommand>();
            CreateMap<IdsWithReasonInput, CancelJobsCommand>();
        }
    }
}
