using System;
using System.Linq;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain.Models;
using AutoMapper.QueryableExtensions;
using Sheaft.Options;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Threading;
using JWT.Algorithms;
using JWT.Builder;

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

        public async Task<string> GetFreshdeskTokenAsync(RequestUser currentUser, CancellationToken token)
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

        public IQueryable<UserDto> GetUser(RequestUser currentUser)
        {
            return _context.Users.OfType<User>()
                    .Get(c => c.Id == currentUser.Id)
                    .ProjectTo<UserDto>(_configurationProvider);
        }

        public IQueryable<UserProfileDto> GetUserProfile(RequestUser currentUser)
        {
            return _context.Users.OfType<User>()
                    .Get(c => c.Id == currentUser.Id)
                    .ProjectTo<UserProfileDto>(_configurationProvider);
        }
    }
}