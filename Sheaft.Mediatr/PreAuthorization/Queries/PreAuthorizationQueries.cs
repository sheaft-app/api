using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Mediatr.PreAuthorization.Queries
{
    public class PreAuthorizationQueries : IPreAuthorizationQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public PreAuthorizationQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<PreAuthorizationDto> GetPreAuthorization(Guid id, RequestUser currentUser)
        {
            return _context.PreAuthorizations
                .Where(c => c.Id == id && c.Order.User.Id == currentUser.Id)
                .ProjectTo<PreAuthorizationDto>(_configurationProvider);
        }

        public IQueryable<PreAuthorizationDto> GetPreAuthorizations(RequestUser currentUser)
        {
            return _context.PreAuthorizations
                .Where(c => c.Order.User.Id == currentUser.Id)
                .ProjectTo<PreAuthorizationDto>(_configurationProvider);
        }
    }
}