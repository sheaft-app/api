using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.StoreProcedures;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Interop;
using Sheaft.Core.Extensions;
using Sheaft.Core.Security;
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

        public async Task<RankInformationDto> UserRankInformationAsync(Guid id, IRequestUser currentUser, CancellationToken token)
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

        public async Task<UserPositionDto> UserPositionInDepartmentAsync(Guid userId, IRequestUser currentUser, CancellationToken token)
        {
            try
            {
                var departement = await _context.Users
                    .Get(u => u.Id == userId && !u.RemovedOn.HasValue)
                    .Select(u => u.Department)
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

        public async Task<UserPositionDto> UserPositionInRegionAsync(Guid userId, IRequestUser currentUser, CancellationToken token)
        {
            try
            {
                var departement = await _context.Users
                    .Get(u => u.Id == userId && !u.RemovedOn.HasValue)
                    .Select(u => u.Department)
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

        public async Task<UserPositionDto> UserPositionInCountryAsync(Guid userId, IRequestUser currentUser, CancellationToken token)
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

        public IQueryable<RegionPointsDto> RegionsPoints(Guid? countryId, IRequestUser currentUser)
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

        public IQueryable<DepartmentPointsDto> DepartmentsPoints(Guid? regionId, IRequestUser currentUser)
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

        public IQueryable<CountryPointsDto> CountriesPoints(Guid? countryId, IRequestUser currentUser)
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

        public IQueryable<CountryUserPointsDto> CountryUsersPoints(Guid? countryId, IRequestUser currentUser)
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

        public IQueryable<RegionUserPointsDto> RegionUsersPoints(Guid? regionId, IRequestUser currentUser)
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

        public IQueryable<DepartmentUserPointsDto> DepartmentUsersPoints(Guid? departmentId, IRequestUser currentUser)
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
        private static UserPositionDto GetAsDto(UserPosition query)
        {
            return new UserPositionDto
                {
                    Points = query.Points,
                    Position = query.Position
                };
        }
        private static IQueryable<CountryPointsDto> GetAsDto(IQueryable<CountryPoints> query)
        {
            return query
                .Select(c => new CountryPointsDto
                {
                    Points = c.Points,
                    Users = c.Users
                });
        }
        private static IQueryable<RegionPointsDto> GetAsDto(IQueryable<RegionPoints> query)
        {
            return query
                .Select(c => new RegionPointsDto
                {
                    Points = c.Points,
                    Users = c.Users,
                    RegionName = c.RegionName,
                    Position = c.Position,
                    RegionId = c.RegionId
                });
        }
        private static IQueryable<DepartmentPointsDto> GetAsDto(IQueryable<DepartmentPoints> query)
        {
            return query
                .Select(c => new DepartmentPointsDto
                {
                    Points = c.Points,
                    Users = c.Users,
                    DepartmentId = c.DepartmentId,
                    DepartmentName = c.DepartmentName,
                    RegionName = c.RegionName,
                    Code = c.Code,
                    Position = c.Position,
                    RegionId = c.RegionId
                });
        }
        private static IQueryable<CountryUserPointsDto> GetAsDto(IQueryable<CountryUserPoints> query)
        {
            return query
                .Select(c => new CountryUserPointsDto
                {
                    Points = c.Points,
                    Name = c.Name,
                    Picture = c.Picture,
                    Position = c.Position,
                    UserId = c.UserId
                });
        }
        private static IQueryable<RegionUserPointsDto> GetAsDto(IQueryable<RegionUserPoints> query)
        {
            return query
                .Select(c => new RegionUserPointsDto
                {
                    Points = c.Points,
                    Name = c.Name,
                    Picture = c.Picture,
                    Position = c.Position,
                    UserId = c.UserId
                });
        }
        private static IQueryable<DepartmentUserPointsDto> GetAsDto(IQueryable<DepartmentUserPoints> query)
        {
            return query
                .Select(c => new DepartmentUserPointsDto
                {
                    Points = c.Points,
                    Name = c.Name,
                    Picture = c.Picture,
                    Position = c.Position,
                    UserId = c.UserId,
                    DepartmentId = c.DepartmentId
                });
        }
    }
}