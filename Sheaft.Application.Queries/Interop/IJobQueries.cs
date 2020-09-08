using Sheaft.Models.Dto;
using Sheaft.Core;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Queries
{
    public interface IJobQueries
    {
        IQueryable<JobDto> GetJob(Guid jobId, RequestUser currentUser);
        IQueryable<JobDto> GetJobs(RequestUser currentUser);
        Task<bool> HasPickingOrdersExportsInProgressAsync(Guid producerId, RequestUser currentUser, CancellationToken token);
        Task<bool> HasProductsImportsInProgressAsync(Guid producerId, RequestUser currentUser, CancellationToken token);
    }
}