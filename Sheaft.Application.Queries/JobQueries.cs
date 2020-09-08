using System;
using System.Linq;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using Sheaft.Core;
using Sheaft.Interop.Enums;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Sheaft.Models.Dto;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure;

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
            try
            {
                return _context.AnyAsync<Job>(r =>
                    !r.Archived &&
                    r.Kind == JobKind.ImportProducts &&
                    (r.Status == ProcessStatusKind.Paused || r.Status == ProcessStatusKind.Processing || r.Status == ProcessStatusKind.Waiting) &&
                    r.User.Id == producerId, token);
            }
            catch (Exception e)
            {
                return Task.FromResult(false);
            }
        }

        public Task<bool> HasPickingOrdersExportsInProgressAsync(Guid producerId, RequestUser currentUser, CancellationToken token)
        {
            try
            {
                return _context.AnyAsync<Job>(r =>
                    !r.Archived &&
                    r.Kind == JobKind.CreatePickingFromOrders &&
                    (r.Status == ProcessStatusKind.Paused || r.Status == ProcessStatusKind.Processing || r.Status == ProcessStatusKind.Waiting) &&                    
                    r.User.Id == producerId, token);
            }
            catch (Exception e)
            {
                return Task.FromResult(false);
            }
        }

        public IQueryable<JobDto> GetJob(Guid jobId, RequestUser currentUser)
        {
            try
            {
                return _context.Jobs
                        .Get(c => c.Id == jobId && c.User.Id == currentUser.Id)
                        .ProjectTo<JobDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<JobDto>().AsQueryable();
            }
        }

        public IQueryable<JobDto> GetJobs(RequestUser currentUser)
        {
            try
            {
                return _context.Jobs
                        .Get(c => c.User.Id == currentUser.Id)
                        .ProjectTo<JobDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<JobDto>().AsQueryable();
            }
        }
    }
}