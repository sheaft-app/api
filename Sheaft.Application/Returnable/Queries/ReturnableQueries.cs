using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Returnable.Queries
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