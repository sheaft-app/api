using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Core;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure;

namespace Sheaft.Application.Queries
{
    public class PackagingQueries : IPackagingQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public PackagingQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<PackagingDto> GetPackaging(Guid id, RequestUser currentUser)
        {
            try
            {
                return _context.Packagings
                        .Get(c => c.Id == id && c.Producer.Id == currentUser.Id)
                        .ProjectTo<PackagingDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<PackagingDto>().AsQueryable();
            }
        }

        public IQueryable<PackagingDto> GetPackagings(RequestUser currentUser)
        {
            try
            {
                return _context.Packagings
                        .Get(c => c.Producer.Id == currentUser.Id)
                        .ProjectTo<PackagingDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<PackagingDto>().AsQueryable();
            }
        }
    }
}