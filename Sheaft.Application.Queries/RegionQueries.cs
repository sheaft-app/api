using System;
using System.Linq;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Core;
using AutoMapper.QueryableExtensions;

namespace Sheaft.Application.Queries
{
    public class RegionQueries : IRegionQueries
    {
        private readonly IAppDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public RegionQueries(IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        public IQueryable<RegionDto> GetRegion(Guid id, RequestUser currentUser)
        {
            return _context.Regions
                    .Where(d => d.Id == id)
                    .ProjectTo<RegionDto>(_configurationProvider);
        }

        public IQueryable<RegionDto> GetRegions(RequestUser currentUser)
        {
            return _context.Regions
                    .ProjectTo<RegionDto>(_configurationProvider);
        }
    }
}