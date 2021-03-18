using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IJobQueries
    {
        IQueryable<JobDto> GetJob(Guid jobId, RequestUser currentUser);
        IQueryable<JobDto> GetJobs(RequestUser currentUser);
        Task<bool> HasPickingOrdersExportsInProgressAsync(Guid producerId, RequestUser currentUser, CancellationToken token);
        Task<bool> HasProductsImportsInProgressAsync(Guid producerId, RequestUser currentUser, CancellationToken token);
    }
}