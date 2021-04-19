using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Mediatr.Job.Queries
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
            return _context.Jobs.AnyAsync(r =>
                !r.Archived &&
                r.Kind == JobKind.ImportProducts &&
                (r.Status == ProcessStatus.Paused || r.Status == ProcessStatus.Processing || r.Status == ProcessStatus.Waiting) &&
                r.User.Id == producerId, token);
        }

        public Task<bool> HasPickingOrdersExportsInProgressAsync(Guid producerId, RequestUser currentUser, CancellationToken token)
        {
            return _context.Jobs.AnyAsync(r =>
                !r.Archived &&
                r.Kind == JobKind.ExportPickingOrders &&
                (r.Status == ProcessStatus.Paused || r.Status == ProcessStatus.Processing || r.Status == ProcessStatus.Waiting) &&
                r.User.Id == producerId, token);
        }

        public IQueryable<JobDto> GetJob(Guid jobId, RequestUser currentUser)
        {
            return _context.Jobs
                    .Where(c => c.Id == jobId && c.User.Id == currentUser.Id)
                    .ProjectTo<JobDto>(_configurationProvider);
        }

        public IQueryable<JobDto> GetJobs(RequestUser currentUser)
        {
            return _context.Jobs
                    .Where(c => c.User.Id == currentUser.Id)
                    .ProjectTo<JobDto>(_configurationProvider);
        }
    }
}