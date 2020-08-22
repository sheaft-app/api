using Sheaft.Models.Dto;
using Sheaft.Core;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface IRegionQueries
    {
        IQueryable<RegionDto> GetRegion(Guid id, RequestUser currentUser);
        IQueryable<RegionDto> GetRegions(RequestUser currentUser);
    }
}