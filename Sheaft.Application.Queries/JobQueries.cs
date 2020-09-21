using System;
using System.Linq;
using Sheaft.Domain.Models;
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Domain.Enums;
using System.Threading.Tasks;
using System.Threading;
using Sheaft.Application.Models;
using AutoMapper.QueryableExtensions;

namespace Sheaft.Application.Queries
{
    public class JobQueries : IJobQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public JobQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public Task<bool> HasProductsImportsInProgressAsync(Guid producerId, RequestUser currentUser, CancellationToken token)
        {
            return _context.AnyAsync<Job>(r =>
                !r.Archived &&
                r.Kind == JobKind.ImportProducts &&
                (r.Status == ProcessStatus.Paused || r.Status == ProcessStatus.Processing || r.Status == ProcessStatus.Waiting) &&
                r.User.Id == producerId, token);
        }

        public Task<bool> HasPickingOrdersExportsInProgressAsync(Guid producerId, RequestUser currentUser, CancellationToken token)
        {
            return _context.AnyAsync<Job>(r =>
                !r.Archived &&
                r.Kind == JobKind.CreatePickingFromOrders &&
                (r.Status == ProcessStatus.Paused || r.Status == ProcessStatus.Processing || r.Status == ProcessStatus.Waiting) &&
                r.User.Id == producerId, token);
        }

        public IQueryable<JobDto> GetJob(Guid jobId, RequestUser currentUser)
        {
            return _context.Jobs
                    .Get(c => c.Id == jobId && c.User.Id == currentUser.Id)
                    .ProjectTo<JobDto>(_configurationProvider);
        }

        public IQueryable<JobDto> GetJobs(RequestUser currentUser)
        {
            return _context.Jobs
                    .Get(c => c.User.Id == currentUser.Id)
                    .ProjectTo<JobDto>(_configurationProvider);
        }
    }
}