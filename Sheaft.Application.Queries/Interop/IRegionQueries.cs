using Sheaft.Models.Dto;
using Sheaft.Interop;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface IRegionQueries
    {
        IQueryable<RegionDto> GetRegion(Guid id, IRequestUser currentUser);
        IQueryable<RegionDto> GetRegions(IRequestUser currentUser);
    }
}