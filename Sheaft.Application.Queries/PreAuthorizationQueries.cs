using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Core;

namespace Sheaft.Application.Queries
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
                    .Get(c => c.Id == id && c.Order.User.Id == currentUser.Id)
                    .ProjectTo<PreAuthorizationDto>(_configurationProvider);
        }

        public IQueryable<PreAuthorizationDto> GetPreAuthorizations(RequestUser currentUser)
        {
            return _context.PreAuthorizations
                    .Get(c => c.Order.User.Id == currentUser.Id)
                    .ProjectTo<PreAuthorizationDto>(_configurationProvider);
        }
    }
}