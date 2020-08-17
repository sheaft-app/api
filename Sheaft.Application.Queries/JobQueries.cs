using System;
using System.Linq;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using Sheaft.Interop;
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

        public Task<bool> HasProductsImportsInProgressAsync(Guid companyId, IRequestUser currentUser, CancellationToken token)
        {
            try
            {
                return _context.AnyAsync<Job>(r =>
                    !r.Archived &&
                    r.Kind == JobKind.ImportProducts &&
                    (r.Status == ProcessStatusKind.Paused || r.Status == ProcessStatusKind.Processing || r.Status == ProcessStatusKind.Waiting) &&
                    r.User.Company != null &&
                    r.User.Company.Id == companyId, token);
            }
            catch (Exception e)
            {
                return Task.FromResult(false);
            }
        }

        public Task<bool> HasPickingOrdersExportsInProgressAsync(Guid companyId, IRequestUser currentUser, CancellationToken token)
        {
            try
            {
                return _context.AnyAsync<Job>(r =>
                    !r.Archived &&
                    r.Kind == JobKind.CreatePickingFromOrders &&
                    (r.Status == ProcessStatusKind.Paused || r.Status == ProcessStatusKind.Processing || r.Status == ProcessStatusKind.Waiting) &&
                    r.User.Company != null &&
                    r.User.Company.Id == companyId, token);
            }
            catch (Exception e)
            {
                return Task.FromResult(false);
            }
        }

        public IQueryable<JobDto> GetJob(Guid jobId, IRequestUser currentUser)
        {
            try
            {
                return _context.Jobs
                        .Get(c =>
                            c.Id == jobId && ((c.User.Company == null && c.User.Id == currentUser.Id) ||
                            (c.User.Company != null && c.User.Company.Id == currentUser.CompanyId)))
                        .ProjectTo<JobDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<JobDto>().AsQueryable();
            }
        }

        public IQueryable<JobDto> GetJobs(IRequestUser currentUser)
        {
            try
            {
                return _context.Jobs
                        .Get(c =>
                            (c.User.Company == null && c.User.Id == currentUser.Id) ||
                            (c.User.Company != null && c.User.Company.Id == currentUser.CompanyId))
                        .ProjectTo<JobDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<JobDto>().AsQueryable();
            }
        }

        private static IQueryable<JobDto> GetAsDto(IQueryable<Job> query)
        {
            return query
                .Select(c => new JobDto
                {
                    Id = c.Id,
                    Archived = c.Archived,
                    CompletedOn = c.CompletedOn,
                    CreatedOn = c.CreatedOn,
                    File = c.File,
                    Message = c.Message,
                    Name = c.Name,
                    RemovedOn = c.RemovedOn,
                    Retried = c.Retried,
                    StartedOn = c.StartedOn,
                    //Status = c.Status,
                    //Kind = c.Kind,
                    UpdatedOn = c.UpdatedOn,
                    //User = new UserProfileDto
                    //{
                    //    Id = c.User.Id,
                    //    Email = c.User.Email,
                    //    Phone = c.User.Phone,
                    //    Kind = c.User.Company == null ? ProfileKind.Consumer : (ProfileKind)c.User.Company.Kind,
                    //    Name = c.User.FirstName + " " + c.User.LastName,
                    //    ShortName = c.User.FirstName + " " + c.User.LastName.Substring(0, 1) + ".",
                    //    Picture = c.User.Picture,
                    //    Initials = c.User.FirstName.Substring(0, 1) + c.User.LastName.Substring(0, 1)
                    //}
                });
        }
    }
}