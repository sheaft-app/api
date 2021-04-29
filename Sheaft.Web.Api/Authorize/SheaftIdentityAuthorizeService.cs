using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.Net.Http;
using IdentityModel.Client;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Caching.Distributed;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Sheaft.Options;

namespace Sheaft.Web.Api.Authorize
{
    public class SheaftIdentityAuthorizeService : IAuthorizationService
    {
        private readonly IAuthorizationPolicyProvider _policyProvider;
        private readonly IAuthorizationHandlerProvider _handlers;
        private readonly IAuthorizationHandlerContextFactory _contextFactory;
        private readonly IAuthorizationEvaluator _evaluator;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _accessor;
        private readonly IOptions<AuthOptions> _authOptions;
        private readonly IDistributedCache _cache;
        private readonly IOptions<CacheOptions> _cacheOptions;

        public SheaftIdentityAuthorizeService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor accessor,
            IAuthorizationPolicyProvider policyProvider,
            IAuthorizationHandlerProvider handlers,
            IAuthorizationHandlerContextFactory contextFactory,
            IAuthorizationEvaluator evaluator,
            IOptions<AuthOptions> authOptions,
            IOptions<CacheOptions> cacheOptions,
            IDistributedCache cache)
        {
            _cache = cache;
            _accessor = accessor;
            _policyProvider = policyProvider;
            _handlers = handlers;
            _contextFactory = contextFactory;
            _evaluator = evaluator;
            _authOptions = authOptions;
            _httpClientFactory = httpClientFactory;
            _cacheOptions = cacheOptions;
        }

        public async Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            var claims = user.Claims.ToList();
            var subClaim = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject);

            // Create a tracking context from the authorization inputs.
            var authContext = _contextFactory.CreateContext(requirements, _accessor.HttpContext.User, resource);

            var cachedUser = user.Identity.IsAuthenticated ? await _cache.GetAsync(user.Identity.Name) : null;
            if (cachedUser != null)
            {
                List<UserClaim> userClaims = null;
                using (var ms = new MemoryStream(cachedUser))
                {
                    userClaims = 
                        await System.Text.Json.JsonSerializer.DeserializeAsync<List<UserClaim>>(ms);
                }

                claims.AddRange(userClaims.Select(c => new Claim(c.ClaimType, c.ClaimValue, c.ClaimValueType, c.ClaimIssuer, c.ClaimOriginalIssuer, subClaim.Subject)));
                _accessor.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, user.Identity.AuthenticationType, JwtClaimTypes.Subject, JwtClaimTypes.Role));
            }
            else
            {
                var _httpClient = _httpClientFactory.CreateClient("identityServer");
                _httpClient.BaseAddress = new Uri(_authOptions.Value.Url);

                _accessor.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues bearer);
                if (bearer.Count > 1)
                {
                    var tokens = bearer[0].Split(" ");
                    if (tokens.Length == 2)
                    {
                        _httpClient.SetToken(tokens[0], tokens[1]);
                        var request = await _httpClient.GetAsync("/connect/userinfo");
                        if (request.IsSuccessStatusCode)
                        {
                            var userInfo = JsonConvert.DeserializeObject<UserInfo>(await request.Content.ReadAsStringAsync());

                            var uClaims = new List<Claim>();
                            if (userInfo.role != null)
                            {
                                var roles = new List<string>();
                                if (userInfo.role is string)
                                {
                                    roles.Add(userInfo.role as string);
                                }
                                else if (userInfo.role is JArray)
                                {
                                    var jarr = (JArray)userInfo.role;
                                    foreach (var content in jarr.ToList())
                                        roles.Add((string)content);
                                }

                                uClaims.AddRange(roles.Select(r => new Claim(JwtClaimTypes.Role, r)));
                            }

                            if (!string.IsNullOrWhiteSpace(userInfo.email))
                                uClaims.Add(new Claim(JwtClaimTypes.Email, userInfo.email, null, subClaim?.Issuer, subClaim?.OriginalIssuer, subClaim?.Subject));

                            if (!string.IsNullOrWhiteSpace(userInfo.family_name))
                                uClaims.Add(new Claim(JwtClaimTypes.FamilyName, userInfo.family_name, null, subClaim?.Issuer, subClaim?.OriginalIssuer, subClaim?.Subject));

                            if (!string.IsNullOrWhiteSpace(userInfo.given_name))
                                uClaims.Add(new Claim(JwtClaimTypes.GivenName, userInfo.given_name, null, subClaim?.Issuer, subClaim?.OriginalIssuer, subClaim?.Subject));

                            if (!string.IsNullOrWhiteSpace(userInfo.name))
                                uClaims.Add(new Claim(JwtClaimTypes.Name, userInfo.name, null, subClaim?.Issuer, subClaim?.OriginalIssuer, subClaim?.Subject));

                            if (!string.IsNullOrWhiteSpace(userInfo.picture))
                                uClaims.Add(new Claim(JwtClaimTypes.Picture, userInfo.picture, null, subClaim?.Issuer, subClaim?.OriginalIssuer, subClaim?.Subject));

                            if (!string.IsNullOrWhiteSpace(userInfo.preferred_username))
                                uClaims.Add(new Claim(JwtClaimTypes.PreferredUserName, userInfo.preferred_username, null, subClaim?.Issuer, subClaim?.OriginalIssuer, subClaim?.Subject));

                            uClaims.Add(new Claim(JwtClaimTypes.EmailVerified, userInfo.email_verified.ToString().ToLower(), null, subClaim?.Issuer, subClaim?.OriginalIssuer, subClaim?.Subject));

                            var buffer = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(uClaims.Select(c => new UserClaim
                            {
                                ClaimIssuer = c.Issuer,
                                ClaimOriginalIssuer = c.OriginalIssuer,
                                ClaimType = c.Type,
                                ClaimValue = c.Value,
                                ClaimValueType = c.ValueType
                            }).ToList());

                            claims.AddRange(uClaims);

                            await _cache.SetAsync(userInfo.sub, buffer, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheOptions.Value.CacheExpirationInMinutes ?? 5), SlidingExpiration = TimeSpan.FromMinutes(_cacheOptions.Value.CacheExpirationInMinutes ?? 5) });
                            _accessor.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, user.Identity.AuthenticationType, JwtClaimTypes.Subject, JwtClaimTypes.Role));
                            
                            // Create a tracking context from the authorization inputs.
                            authContext = _contextFactory.CreateContext(requirements, _accessor.HttpContext.User, resource);
                        }
                        else
                        {
                            authContext.Fail();
                        }
                    }
                }
            }
            // By default this returns an IEnumerable<IAuthorizationHandlers> from DI.
            var handlers = await _handlers.GetHandlersAsync(authContext);

            // Invoke all handlers.
            foreach (var handler in handlers)
                await handler.HandleAsync(authContext);

            // Check the context, by default success is when all requirements have been met.
            return _evaluator.Evaluate(authContext);
        }

        public async Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
        {
            var policy = await _policyProvider.GetPolicyAsync(policyName);
            return await AuthorizeAsync(user, resource, policy.Requirements);
        }

        [Serializable]
        private class UserClaim
        {
            public string ClaimType { get; set; }
            public string ClaimValue { get; set; }
            public string ClaimValueType { get; set; }
            public string ClaimIssuer { get; set; }
            public string ClaimOriginalIssuer { get; set; }
        }

        [Serializable]
        private class UserInfo
        {
            public string email { get; set; }
            public bool email_verified { get; set; }
            public string family_name { get; set; }
            public string given_name { get; set; }
            public string name { get; set; }
            public string picture { get; set; }
            public string preferred_username { get; set; }
            public object role { get; set; }
            public string sub { get; set; }
        }
    }
}
