using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Region.Queries
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