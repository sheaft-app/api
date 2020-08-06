using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Sheaft.Options;
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

namespace Sheaft.Api.Authorize
{

    public class SheaftIdentityAuthorizeService : IAuthorizationService
    {
        private readonly IAuthorizationPolicyProvider _policyProvider;
        private readonly IAuthorizationHandlerProvider _handlers;
        private readonly ILogger<DefaultAuthorizationService> _logger;
        private readonly IAuthorizationHandlerContextFactory _contextFactory;
        private readonly IAuthorizationEvaluator _evaluator;
        private readonly IOptions<AuthorizationOptions> _options;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _accessor;
        private readonly IOptions<AuthOptions> _authOptions;

        public SheaftIdentityAuthorizeService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor accessor,
            IAuthorizationPolicyProvider policyProvider,
            IAuthorizationHandlerProvider handlers,
            ILogger<DefaultAuthorizationService> logger,
            IAuthorizationHandlerContextFactory contextFactory,
            IAuthorizationEvaluator evaluator,
            IOptions<AuthorizationOptions> options,
            IOptions<AuthOptions> authOptions)
        {
            _accessor = accessor;
            _policyProvider = policyProvider;
            _handlers = handlers;
            _logger = logger;
            _contextFactory = contextFactory;
            _evaluator = evaluator;
            _options = options;
            _authOptions = authOptions;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            var _httpClient = _httpClientFactory.CreateClient("identityServer");
            _httpClient.BaseAddress = new Uri(_authOptions.Value.Url);

            _accessor.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues bearer);
            if (bearer.Any())
            {
                var tokens = bearer[0].Split(" ");
                if (tokens.Length == 2)
                {
                    _httpClient.SetToken(tokens[0], tokens[1]);
                    var request = await _httpClient.GetAsync("/connect/userinfo");
                    if (request.IsSuccessStatusCode)
                    {
                        var userInfo = JsonConvert.DeserializeObject<UserInfo>(await request.Content.ReadAsStringAsync());

                        var claims = user.Claims.ToList();
                        if (userInfo.role != null)
                        {
                            var roles = new List<string>();
                            if (userInfo.role is string)
                            {
                                roles.Add(userInfo.role as string);
                            }
                            else if(userInfo.role is JArray)
                            {
                                var jarr = (JArray)userInfo.role;
                                foreach (var content in jarr.ToList())
                                {
                                    roles.Add((string)content); 
                                }
                            }

                            claims.AddRange(roles.Select(r => new Claim(JwtClaimTypes.Role, r)));
                        }

                        if(!string.IsNullOrWhiteSpace(userInfo.email))
                            claims.Add(new Claim(JwtClaimTypes.Email, userInfo.email));

                        if (!string.IsNullOrWhiteSpace(userInfo.family_name))
                            claims.Add(new Claim(JwtClaimTypes.FamilyName, userInfo.family_name));

                        if (!string.IsNullOrWhiteSpace(userInfo.given_name))
                            claims.Add(new Claim(JwtClaimTypes.GivenName, userInfo.given_name));

                        if (!string.IsNullOrWhiteSpace(userInfo.name))
                            claims.Add(new Claim(JwtClaimTypes.Name, userInfo.name));

                        if (!string.IsNullOrWhiteSpace(userInfo.picture))
                            claims.Add(new Claim(JwtClaimTypes.Picture, userInfo.picture));

                        if (!string.IsNullOrWhiteSpace(userInfo.preferred_username))
                            claims.Add(new Claim(JwtClaimTypes.PreferredUserName, userInfo.preferred_username));

                        claims.Add(new Claim(JwtClaimTypes.EmailVerified, userInfo.email_verified.ToString().ToLower()));

                        _accessor.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, user.Identity.AuthenticationType, JwtClaimTypes.Subject, JwtClaimTypes.Role));
                    }
                }
            }

            // Create a tracking context from the authorization inputs.
            var authContext = _contextFactory.CreateContext(requirements, _accessor.HttpContext.User, resource);

            // By default this returns an IEnumerable<IAuthorizationHandlers> from DI.
            var handlers = await _handlers.GetHandlersAsync(authContext);

            // Invoke all handlers.
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(authContext);
            }

            // Check the context, by default success is when all requirements have been met.
            return _evaluator.Evaluate(authContext);
        }

        public async Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
        {
            var policy = await _policyProvider.GetPolicyAsync(policyName);
            return await AuthorizeAsync(user, resource, policy.Requirements);
        }
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
