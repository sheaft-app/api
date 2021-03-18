using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IRegionQueries
    {
        IQueryable<RegionDto> GetRegion(Guid id, RequestUser currentUser);
        IQueryable<RegionDto> GetRegions(RequestUser currentUser);
    }
}