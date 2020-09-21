using System;
using System.Linq;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Core;
using AutoMapper.QueryableExtensions;

namespace Sheaft.Application.Queries
{
    public class ReturnableQueries : IReturnableQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public ReturnableQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<ReturnableDto> GetReturnable(Guid id, RequestUser currentUser)
        {
            return _context.Returnables
                    .Get(c => c.Id == id && c.Producer.Id == currentUser.Id)
                    .ProjectTo<ReturnableDto>(_configurationProvider);
        }

        public IQueryable<ReturnableDto> GetReturnables(RequestUser currentUser)
        {
            return _context.Returnables
                    .Get(c => c.Producer.Id == currentUser.Id)
                    .ProjectTo<ReturnableDto>(_configurationProvider);
        }
    }
}