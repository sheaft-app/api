using System;
using System.Linq;
using System.Net.Http;
using Sheaft.Application.Interop;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain.Models;
using AutoMapper.QueryableExtensions;
using Sheaft.Options;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Threading;
using IdentityModel.Client;
using JWT.Algorithms;
using JWT.Builder;
using Newtonsoft.Json;

namespace Sheaft.Application.Queries
{
    public class UserQueries : IUserQueries
    {
        private readonly IAppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly SireneOptions _sireneOptions;
        private readonly FreshdeskOptions _freshdeskOptions;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public UserQueries(
            IOptionsSnapshot<FreshdeskOptions> freshdeskOptions,
            IHttpClientFactory httpClientFactory,
            IOptionsSnapshot<SireneOptions> sireneOptions,
            IAppDbContext context, 
            AutoMapper.IConfigurationProvider configurationProvider)
        {
            _freshdeskOptions = freshdeskOptions.Value;
            _configurationProvider = configurationProvider;
            _context = context;
            
            _sireneOptions = sireneOptions.Value;

            _httpClient = httpClientFactory.CreateClient("sirene");
            _httpClient.BaseAddress = new Uri(_sireneOptions.Url);
            _httpClient.SetToken(_sireneOptions.Scheme, _sireneOptions.ApiKey);
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

        public IQueryable<UserDto> GetUser(Guid id, RequestUser currentUser)
        {
            return _context.Users.OfType<User>()
                    .Get(c => c.Id == id)
                    .ProjectTo<UserDto>(_configurationProvider);
        }

        public IQueryable<UserProfileDto> GetUserProfile(Guid id, RequestUser currentUser)
        {
            return _context.Users.OfType<User>()
                    .Get(c => c.Id == id)
                    .ProjectTo<UserProfileDto>(_configurationProvider);
        }

        public IQueryable<BusinessProfileDto> GetMyProfile(RequestUser currentUser)
        {
            return _context.Users.OfType<Business>()
                .Get(c => c.Id == currentUser.Id)
                .ProjectTo<BusinessProfileDto>(_configurationProvider);
        }

        public async Task<SirenBusinessDto> RetrieveSiretInfosAsync(string siret, RequestUser currentUser, CancellationToken token)
        {
            var result = await _httpClient.GetAsync(string.Format(_sireneOptions.SearchSiretUrl, siret), token);
            if (!result.IsSuccessStatusCode)
                return null;

            var content = await result.Content.ReadAsStringAsync();
            var contentObj = JsonConvert.DeserializeObject<SirenBusinessResult>(content);
            return contentObj.Etablissement;
        }

        private class SirenBusinessResult
        {
            public SirenBusinessDto Etablissement { get; set; }
        }
    }
}