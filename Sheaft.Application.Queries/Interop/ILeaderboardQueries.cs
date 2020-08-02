using Sheaft.Domain.Models;
using Sheaft.Domain.StoreProcedures;
using Sheaft.Domain.Views;
using Sheaft.Models.Dto;
using Sheaft.Interop;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Queries
{
    public interface ILeaderboardQueries
    {
        IQueryable<CountryPointsDto> CountriesPoints(Guid? countryId, IRequestUser currentUser);
        IQueryable<CountryUserPointsDto> CountryUsersPoints(Guid? countryId, IRequestUser currentUser);
        IQueryable<DepartmentPointsDto> DepartmentsPoints(Guid? regionId, IRequestUser currentUser);
        IQueryable<DepartmentUserPointsDto> DepartmentUsersPoints(Guid? departmentId, IRequestUser currentUser);
        IQueryable<RegionPointsDto> RegionsPoints(Guid? countryId, IRequestUser currentUser);
        IQueryable<RegionUserPointsDto> RegionUsersPoints(Guid? regionId, IRequestUser currentUser);
        Task<UserPositionDto> UserPositionInCountryAsync(Guid userId, IRequestUser currentUser, CancellationToken token);
        Task<UserPositionDto> UserPositionInDepartmentAsync(Guid userId, IRequestUser currentUser, CancellationToken token);
        Task<UserPositionDto> UserPositionInRegionAsync(Guid userId, IRequestUser currentUser, CancellationToken token);
        Task<RankInformationDto> UserRankInformationAsync(Guid userId, IRequestUser currentUser, CancellationToken token);
    }
}