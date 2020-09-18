using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Core;
using Sheaft.Infrastructure;

namespace Sheaft.Application.Queries
{
    public class UboQueries : IUboQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public UboQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<UboDto> GetUbo(Guid id, RequestUser currentUser)
        {
            return _context.Ubos
                    .Get(c => c.Id == id)
                    .ProjectTo<UboDto>(_configurationProvider);
        }

        public IQueryable<UboDto> GetUbos(RequestUser currentUser)
        {
            return _context.Ubos.Get(null)
                    .ProjectTo<UboDto>(_configurationProvider);
        }
    }
}