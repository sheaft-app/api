using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IRegionQueries
    {
        IQueryable<RegionDto> GetRegion(Guid id, RequestUser currentUser);
        IQueryable<RegionDto> GetRegions(RequestUser currentUser);
    }
}