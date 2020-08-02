using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Interop;
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

        public IQueryable<RegionDto> GetRegion(Guid id, IRequestUser currentUser)
        {
            try
            {
                return _context.Regions
                        .Get(d => d.Id == id)
                        .ProjectTo<RegionDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<RegionDto>().AsQueryable();
            }
        }

        public IQueryable<RegionDto> GetRegions(IRequestUser currentUser)
        {
            try
            {
                return _context.Regions
                        .Get()
                        .ProjectTo<RegionDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<RegionDto>().AsQueryable();
            }
        }

        private static IQueryable<RegionDto> GetAsDto(IQueryable<Region> query)
        {
            return query
                .Select(c => new RegionDto
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name
                });
        }
    }
}