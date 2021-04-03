using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Options;

namespace Sheaft.Mediatr.BusinessClosing.Queries
{
    public class BusinessClosingQueries : IBusinessClosingQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        private readonly RoleOptions _roleOptions;

        public BusinessClosingQueries(
            IAppDbContext context, 
            IOptionsSnapshot<RoleOptions> roleOptions,
            AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _roleOptions = roleOptions.Value;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<ClosingDto> GetClosing(Guid id, RequestUser currentUser)
        {
            if (currentUser.IsInRole(_roleOptions.Owner.Value))
            {
                return _context.Set<Domain.Business>()
                    .Get(b => b.Id == currentUser.Id)
                    .Select(b => b.Closings.SingleOrDefault(c => c.Id == id))
                    .ProjectTo<ClosingDto>(_configurationProvider);
            }
            
            return _context.Set<Domain.BusinessClosing>()
                .Where(c => c.Id == id)
                .ProjectTo<ClosingDto>(_configurationProvider);
        }

        public IQueryable<ClosingDto> GetClosings(RequestUser currentUser)
        {
            if (currentUser.IsInRole(_roleOptions.Owner.Value))
            {
                return _context.Set<Domain.Business>()
                    .Get(b => b.Id == currentUser.Id)
                    .SelectMany(b => b.Closings)
                    .ProjectTo<ClosingDto>(_configurationProvider);
            }
            
            return _context.Set<Domain.BusinessClosing>()
                .ProjectTo<ClosingDto>(_configurationProvider);
        }
    }
}