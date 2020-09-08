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
    public class ConsumerQueries : IConsumerQueries
    {
        private readonly IAppDbContext _context;
        private readonly FreshdeskOptions _freshdeskOptions;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public ConsumerQueries(IOptionsSnapshot<FreshdeskOptions> freshdeskOptions, IAppDbContext context, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _freshdeskOptions = freshdeskOptions.Value;
            _configurationProvider = configurationProvider;
            _context = context;
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

        public IQueryable<ConsumerDto> GetConsumer(Guid id, RequestUser currentUser)
        {
            try
            {
                return _context.Users.OfType<Consumer>()
                        .Get(c => c.Id == id)
                        .ProjectTo<ConsumerDto>(_configurationProvider);
            }
            catch (Exception e)
            {
                return new List<ConsumerDto>().AsQueryable();
            }
        }
    }
}