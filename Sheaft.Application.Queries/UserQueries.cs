using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JWT.Algorithms;
using JWT.Builder;
using Sheaft.Infrastructure.Interop;
using Sheaft.Models.Dto;
using Sheaft.Core;
using Sheaft.Options;
using Microsoft.Extensions.Options;
using Sheaft.Domain.Models;
using Sheaft.Interop.Enums;
using AutoMapper.QueryableExtensions;
using Sheaft.Infrastructure;

namespace Sheaft.Application.Queries
{
    public class UserQueries : IUserQueries
    {
        private readonly IAppDbContext _context;
        private readonly FreshdeskOptions _freshdeskOptions;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public UserQueries(IOptionsSnapshot<FreshdeskOptions> freshdeskOptions, IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _freshdeskOptions = freshdeskOptions.Value;
            _configurationProvider = configurationProvider;
            _context = context;
        }

        public IQueryable<UserDto> GetUserProfile(Guid id, RequestUser currentUser)
        {
            try
            {
                return _context.Users
                        .Get(c => c.Id == id)
                        .ProjectTo<UserDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<UserDto>().AsQueryable();
            }
        }

        public async Task<string> GetFreshdeskTokenAsync(RequestUser currentUser, CancellationToken token)
        {
            try
            {
                var jwtToken = new JwtBuilder()
                 .WithAlgorithm(new HMACSHA256Algorithm())
                 .WithSecret(_freshdeskOptions.ApiKey)
                 .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                 .AddClaim("name", currentUser.Name)
                 .AddClaim("email", currentUser.Email)
                 .Encode();

                return await Task.FromResult(jwtToken);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IQueryable<UserDto> GetUser(Guid id, RequestUser currentUser)
        {
            try
            {
                return _context.Users
                        .Get(c => c.Id == id && c.Company.Id == currentUser.CompanyId)
                        .ProjectTo<UserDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<UserDto>().AsQueryable();
            }
        }

        public IQueryable<UserDto> GetUsers(RequestUser currentUser)
        {
            try
            {
                return _context.Users
                        .Get(c => c.Company.Id == currentUser.CompanyId)
                        .ProjectTo<UserDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<UserDto>().AsQueryable();
            }
        }

        private static IQueryable<UserProfileDto> GetAsDto(IQueryable<User> query)
        {
            return query
                .Select(c => new UserProfileDto
                {
                    Id = c.Id,
                    Email = c.Email,
                    Phone = c.Phone,
                    Picture = c.Picture,
                    Name = c.FirstName + " " + c.LastName,
                    ShortName = c.FirstName + " " + c.LastName.Substring(0, 1) + ".",
                    Initials = c.FirstName.Substring(0, 1) + c.LastName.Substring(0, 1),
                });
        }

        private static IQueryable<UserDto> GetAsProfileDto(IQueryable<User> query)
        {
            return query
                .Select(c => new UserDto
                {
                    Id = c.Id,
                    Email = c.Email,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Phone = c.Phone,
                    Name = c.FirstName + " " + c.LastName,
                    ShortName = c.FirstName + " " + c.LastName.Substring(0, 1) + ".",
                    Picture = c.Picture,
                    Kind = c.Company == null ? ProfileKind.Consumer : (ProfileKind)c.Company.Kind,
                    Initials = c.FirstName.Substring(0, 1) + c.LastName.Substring(0, 1),
                    Anonymous = c.Anonymous,
                    CreatedOn = c.CreatedOn,
                    UpdatedOn = c.UpdatedOn,
                    Department = new DepartmentDto
                    {
                        Id = c.Department.Id,
                        Code = c.Department.Code,
                        Name = c.Department.Name
                    }
                });
        }
    }
}