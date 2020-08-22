﻿using Sheaft.Models.Dto;
using Sheaft.Core;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Queries
{
    public interface ILeaderboardQueries
    {
        IQueryable<CountryPointsDto> CountriesPoints(Guid? countryId, RequestUser currentUser);
        IQueryable<CountryUserPointsDto> CountryUsersPoints(Guid? countryId, RequestUser currentUser);
        IQueryable<DepartmentPointsDto> DepartmentsPoints(Guid? regionId, RequestUser currentUser);
        IQueryable<DepartmentUserPointsDto> DepartmentUsersPoints(Guid? departmentId, RequestUser currentUser);
        IQueryable<RegionPointsDto> RegionsPoints(Guid? countryId, RequestUser currentUser);
        IQueryable<RegionUserPointsDto> RegionUsersPoints(Guid? regionId, RequestUser currentUser);
        Task<UserPositionDto> UserPositionInCountryAsync(Guid userId, RequestUser currentUser, CancellationToken token);
        Task<UserPositionDto> UserPositionInDepartmentAsync(Guid userId, RequestUser currentUser, CancellationToken token);
        Task<UserPositionDto> UserPositionInRegionAsync(Guid userId, RequestUser currentUser, CancellationToken token);
        Task<RankInformationDto> UserRankInformationAsync(Guid userId, RequestUser currentUser, CancellationToken token);
    }
}