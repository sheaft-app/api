using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Interfaces;
using Sheaft.Application.Common.Interfaces.Queries;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Options;
using Sheaft.Domain;

namespace Sheaft.Application.Leaderboard.Queries
{
    public class LeaderboardQueries : ILeaderboardQueries
    {
        private readonly IAppDbContext _context;
        private readonly IDapperContext _dapper;
        private readonly ScoringOptions _scoringOptions;
        private readonly RoleOptions _roleOptions;
        private readonly AutoMapper.IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public LeaderboardQueries(
            IOptionsSnapshot<ScoringOptions> scoringOptions,
            IOptionsSnapshot<RoleOptions> roleOptions,
            IAppDbContext context,
            IDapperContext dapper,
            AutoMapper.IMapper mapper,
            AutoMapper.IConfigurationProvider configurationProvider)
        {
            _roleOptions = roleOptions.Value;
            _mapper = mapper;
            _configurationProvider = configurationProvider;
            _scoringOptions = scoringOptions.Value;
            _context = context;
            _dapper = dapper;
        }

        public async Task<RankInformationDto> UserRankInformationAsync(Guid id, RequestUser currentUser, CancellationToken token)
        {
            if (!currentUser.IsInRole(_roleOptions.Consumer.Value))
                return null;

            var user = await _context.FindByIdAsync<Domain.User>(id, token);
            if (user == null)
                return null;

            return new RankInformationDto(_scoringOptions.Ranks, user.TotalPoints);
        }

        public async Task<UserPositionDto> UserPositionInDepartmentAsync(Guid userId, RequestUser currentUser, CancellationToken token)
        {
            var departement = await _context.Users.OfType<Domain.User>()
                .Get(u => u.Id == userId && !u.RemovedOn.HasValue)
                .Select(u => u.Address.Department)
                .SingleOrDefaultAsync(token);

            if (departement == null)
                return null;

            return _mapper.Map<UserPositionDto>(await _dapper.GetUserPositionInDepartmentAsync(userId, departement.Id));
        }

        public async Task<UserPositionDto> UserPositionInRegionAsync(Guid userId, RequestUser currentUser, CancellationToken token)
        {
            var departement = await _context.Users.OfType<Domain.User>()
                .Get(u => u.Id == userId && !u.RemovedOn.HasValue)
                .Select(u => u.Address.Department)
                .SingleOrDefaultAsync(token);

            if (departement?.Region == null)
                return null;

            return _mapper.Map<UserPositionDto>(await _dapper.GetUserPositionInRegionAsync(userId, departement.Region.Id));
        }

        public async Task<UserPositionDto> UserPositionInCountryAsync(Guid userId, RequestUser currentUser, CancellationToken token)
        {
            return _mapper.Map<UserPositionDto>(await _dapper.GetUserPositionAsync(userId));
        }

        public IQueryable<RegionPointsDto> RegionsPoints(Guid? countryId, RequestUser currentUser)
        {
            return _context.RegionPoints
                    .AsNoTracking()
                    .ProjectTo<RegionPointsDto>(_configurationProvider);
        }

        public IQueryable<DepartmentPointsDto> DepartmentsPoints(Guid? regionId, RequestUser currentUser)
        {
            var query = _context.DepartmentPoints.AsQueryable();
            if (regionId.HasValue)
                query = query.Where(q => q.RegionId == regionId);

            return query
                    .AsNoTracking()
                    .ProjectTo<DepartmentPointsDto>(_configurationProvider);
        }

        public IQueryable<CountryPointsDto> CountriesPoints(Guid? countryId, RequestUser currentUser)
        {
            return _context.CountryPoints
                    .AsNoTracking()
                    .ProjectTo<CountryPointsDto>(_configurationProvider);
        }

        public IQueryable<CountryUserPointsDto> CountryUsersPoints(Guid? countryId, RequestUser currentUser)
        {
            return _context.CountryUserPoints
                    .AsNoTracking()
                    .ProjectTo<CountryUserPointsDto>(_configurationProvider);
        }

        public IQueryable<RegionUserPointsDto> RegionUsersPoints(Guid? regionId, RequestUser currentUser)
        {
            var query = _context.RegionUserPoints.AsQueryable();
            if (regionId.HasValue)
                query = query.Where(q => q.RegionId == regionId);

            return query
                    .AsNoTracking()
                    .ProjectTo<RegionUserPointsDto>(_configurationProvider);
        }

        public IQueryable<DepartmentUserPointsDto> DepartmentUsersPoints(Guid? departmentId, RequestUser currentUser)
        {
            var query = _context.DepartmentUserPoints.AsQueryable();
            if (departmentId.HasValue)
                query = query.Where(q => q.DepartmentId == departmentId);

            return query
                    .AsNoTracking()
                    .ProjectTo<DepartmentUserPointsDto>(_configurationProvider);
        }
    }
}