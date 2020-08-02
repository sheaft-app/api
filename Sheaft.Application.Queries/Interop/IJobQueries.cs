using Sheaft.Models.Dto;
using Sheaft.Interop;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Queries
{
    public interface IJobQueries
    {
        IQueryable<JobDto> GetJob(Guid jobId, IRequestUser currentUser);
        IQueryable<JobDto> GetJobs(IRequestUser currentUser);
        Task<bool> HasPickingOrdersExportsInProgressAsync(Guid companyId, IRequestUser currentUser, CancellationToken token);
        Task<bool> HasProductsImportsInProgressAsync(Guid companyId, IRequestUser currentUser, CancellationToken token);
    }
}