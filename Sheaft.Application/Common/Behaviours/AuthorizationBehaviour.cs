using MediatR;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Sheaft.Core;
using Sheaft.Core.Extensions;
using Sheaft.Exceptions;
using AuthorizeAttribute = CleanArchitecture.Application.Common.Security.AuthorizeAttribute;

namespace CleanArchitecture.Application.Common.Behaviours
{
    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ITrackedUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationBehaviour(
            IHttpContextAccessor httpContextAccessor,
            IAuthorizationService authorizationService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authorizationService = authorizationService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();
            if (authorizeAttributes.Any())
            {
                if (!request.RequestUser.IsAuthenticated)
                    throw new UnauthorizedAccessException();

                // Role-based authorization
                var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));
                if (authorizeAttributesWithRoles.Any())
                {
                    foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
                    {
                        var authorized = false;
                        foreach (var role in roles)
                        {
                            var isInRole = request.RequestUser.IsInRole(role.Trim());
                            if (!isInRole)
                                continue;
                            
                            authorized = true;
                            break;
                        }

                        if (!authorized)
                            throw new ForbiddenException();
                    }
                }

                // Policy-based authorization
                var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));
                if (authorizeAttributesWithPolicies.Any())
                {
                    foreach(var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
                    {
                        var authorized = await _authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext?.User, policy);
                        if (!authorized.Succeeded)
                            throw new ForbiddenException();
                    }
                }
            }

            // User is authorized / authorization not required
            return await next();
        }
    }
}
