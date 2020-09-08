using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.StoreProcedures;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Core;
using Sheaft.Core.Extensions;
using Sheaft.Options;
using Microsoft.Extensions.Options;
using Sheaft.Domain.Views;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Domain.Models;

namespace Sheaft.Application.Queries
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
            try
            {
                if (!currentUser.IsInRole(_roleOptions.Consumer.Value))
                    return null;

                var user = await _context.FindByIdAsync<User>(id, token);
                if (user == null)
                    return null;

                return new RankInformationDto(_scoringOptions.Ranks, user.TotalPoints);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<UserPositionDto> UserPositionInDepartmentAsync(Guid userId, RequestUser currentUser, CancellationToken token)
        {
            try
            {
                var departement = await _context.Users.OfType<User>()
                    .Get(u => u.Id == userId && !u.RemovedOn.HasValue)
                    .Select(u => u.Address.Department)
                    .SingleOrDefaultAsync(token);

                if (departement == null)
                    return null;

                return _mapper.Map<UserPositionDto>(await _dapper.GetUserPositionInDepartmentAsync(userId, departement.Id));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<UserPositionDto> UserPositionInRegionAsync(Guid userId, RequestUser currentUser, CancellationToken token)
        {
            try
            {
                var departement = await _context.Users.OfType<User>()
                    .Get(u => u.Id == userId && !u.RemovedOn.HasValue)
                    .Select(u => u.Address.Department)
                    .SingleOrDefaultAsync(token);

                if (departement?.Region == null)
                    return null;

                return _mapper.Map<UserPositionDto>(await _dapper.GetUserPositionInRegionAsync(userId, departement.Region.Id));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<UserPositionDto> UserPositionInCountryAsync(Guid userId, RequestUser currentUser, CancellationToken token)
        {
            try
            {
                return _mapper.Map<UserPositionDto>(await _dapper.GetUserPositionAsync(userId));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IQueryable<RegionPointsDto> RegionsPoints(Guid? countryId, RequestUser currentUser)
        {
            try
            {
                return _context.RegionPoints
                        .AsNoTracking()
                        .ProjectTo<RegionPointsDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<RegionPointsDto>().AsQueryable();
            }
        }

        public IQueryable<DepartmentPointsDto> DepartmentsPoints(Guid? regionId, RequestUser currentUser)
        {
            try
            {
                var query = _context.DepartmentPoints.AsQueryable();
                if (regionId.HasValue)
                    query = query.Where(q => q.RegionId == regionId);

                return query
                        .AsNoTracking()
                        .ProjectTo<DepartmentPointsDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<DepartmentPointsDto>().AsQueryable();
            }
        }

        public IQueryable<CountryPointsDto> CountriesPoints(Guid? countryId, RequestUser currentUser)
        {
            try
            {
                return _context.CountryPoints
                        .AsNoTracking()
                        .ProjectTo<CountryPointsDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<CountryPointsDto>().AsQueryable();
            }
        }

        public IQueryable<CountryUserPointsDto> CountryUsersPoints(Guid? countryId, RequestUser currentUser)
        {
            try
            {
                return _context.CountryUserPoints
                        .AsNoTracking()
                        .ProjectTo<CountryUserPointsDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<CountryUserPointsDto>().AsQueryable();
            }
        }

        public IQueryable<RegionUserPointsDto> RegionUsersPoints(Guid? regionId, RequestUser currentUser)
        {
            try
            {
                var query = _context.RegionUserPoints.AsQueryable();
                if (regionId.HasValue)
                    query = query.Where(q => q.RegionId == regionId);

                return query
                        .AsNoTracking()
                        .ProjectTo<RegionUserPointsDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<RegionUserPointsDto>().AsQueryable();
            }
        }

        public IQueryable<DepartmentUserPointsDto> DepartmentUsersPoints(Guid? departmentId, RequestUser currentUser)
        {
            try
            {
                var query = _context.DepartmentUserPoints.AsQueryable();
                if (departmentId.HasValue)
                    query = query.Where(q => q.DepartmentId == departmentId);

                return query
                        .AsNoTracking()
                        .ProjectTo<DepartmentUserPointsDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<DepartmentUserPointsDto>().AsQueryable();
            }
        }
    }
}