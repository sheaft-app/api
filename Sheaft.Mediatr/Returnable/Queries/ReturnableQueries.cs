using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Mediatr.Returnable.Queries
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
                .Where(c => c.Id == id && c.ProducerId == currentUser.Id)
                .ProjectTo<ReturnableDto>(_configurationProvider);
        }

        public IQueryable<ReturnableDto> GetReturnables(RequestUser currentUser)
        {
            return _context.Returnables
                .Where(c => c.ProducerId == currentUser.Id)
                .ProjectTo<ReturnableDto>(_configurationProvider);
        }
    }
}