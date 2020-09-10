using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Core;
using Sheaft.Domain.Models;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure;

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
            try
            {
                return _context.Regions
                        .Where(d => d.Id == id)
                        .ProjectTo<RegionDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<RegionDto>().AsQueryable();
            }
        }

        public IQueryable<RegionDto> GetRegions(RequestUser currentUser)
        {
            try
            {
                return _context.Regions
                        .ProjectTo<RegionDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<RegionDto>().AsQueryable();
            }
        }
    }
}