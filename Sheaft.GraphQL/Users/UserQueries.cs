using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using IdentityModel.Client;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Models;
using Sheaft.Application.Security;
using Sheaft.Core.Options;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Users
{
    [ExtendObjectType(Name = "Query")]
    public class UserQueries : SheaftQuery
    {
        private readonly HttpClient _httpClient;
        private readonly SireneOptions _sireneOptions;
        private readonly FreshdeskOptions _freshdeskOptions;
        
        public UserQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor,
            IOptionsSnapshot<FreshdeskOptions> freshdeskOptions,
            IHttpClientFactory httpClientFactory,
            IOptionsSnapshot<SireneOptions> sireneOptions)
            :base(currentUserService, httpContextAccessor)
        {
            _freshdeskOptions = freshdeskOptions.Value;
            _sireneOptions = sireneOptions.Value;

            _httpClient = httpClientFactory.CreateClient("sirene");
            _httpClient.BaseAddress = new Uri(_sireneOptions.Url);
            _httpClient.SetToken(_sireneOptions.Scheme, _sireneOptions.ApiKey);
        }

        [GraphQLName("me")]
        [GraphQLType(typeof(UserType))]
        [UseDbContext(typeof(QueryDbContext))]
        [UseSingleOrDefault]
        public IQueryable<User> Get([ScopedService] QueryDbContext context)
        {
            SetLogTransaction();
            
            if (!CurrentUser.IsAuthenticated())
                return null;
            
            return context.Users
                .Where(c => c.Id == CurrentUser.Id);
        }

        [GraphQLName("freshdeskToken")]
        [GraphQLType(typeof(StringType))]
        [Authorize(Policy = Policies.AUTHENTICATED)]
        public async Task<string> GetFreshdeskToken()
        {
            SetLogTransaction();
            
            if (!CurrentUser.IsAuthenticated())
                return null;
            
            var jwtToken = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(_freshdeskOptions.ApiKey)
                .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                .AddClaim("name", CurrentUser.Name)
                .AddClaim("email", CurrentUser.Email)
                .Encode();

            return await Task.FromResult(jwtToken);
        }

        [GraphQLName("siretInfo")]
        [GraphQLType(typeof(SirenBusinessDtoType))]
        public async Task<SirenBusinessDto> RetrieveSiretInfosAsync(string siret, CancellationToken token)
        {
            SetLogTransaction(siret);
            
            var result = await _httpClient.GetAsync(string.Format(_sireneOptions.SearchSiretUrl, siret), token);
            if (!result.IsSuccessStatusCode)
                return null;

            var content = await result.Content.ReadAsStringAsync(token);
            var contentObj = JsonConvert.DeserializeObject<SirenBusinessResult>(content);
            return contentObj.Etablissement;
        }

        private class SirenBusinessResult
        {
            public SirenBusinessDto Etablissement { get; set; }
        }
    }
}