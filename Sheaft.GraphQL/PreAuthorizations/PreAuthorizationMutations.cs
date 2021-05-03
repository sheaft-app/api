using System;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.Card.Commands;
using Sheaft.Mediatr.PreAuthorization.Commands;

namespace Sheaft.GraphQL.PreAuthorizations
{
    [ExtendObjectType(Name = "Mutation")]
    public class PreAuthorizationMutations : SheaftMutation
    {
        public PreAuthorizationMutations(
            ISheaftMediatr mediator,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(mediator, currentUserService, httpContextAccessor)
        {
        }
        
        [GraphQLName("createCardRegistration")]
        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLType(typeof(CardRegistrationDtoType))]
        public async Task<CardRegistrationDto> CreateCardRegistration(CancellationToken token)
        {
            return await ExecuteAsync<CreateCardRegistrationCommand, CardRegistrationDto>(new CreateCardRegistrationCommand(CurrentUser), token);
        }
        
        [GraphQLName("prepayOrder")]
        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLType(typeof(PreAuthorizationType))]
        public async Task<PreAuthorization> CreatePreAuthorization([GraphQLName("input")] CreatePreAuthorizationForOrderCommand input,
            PreAuthorizationsByIdBatchDataLoader preAuthorizationsDataLoader, CancellationToken token)
        {
            input.IpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            input.BrowserInfo.AcceptHeader = _httpContextAccessor.HttpContext.Request.Headers["Accept"];
            
            var result = await ExecuteAsync<CreatePreAuthorizationForOrderCommand, Guid>(input, token);
            return await preAuthorizationsDataLoader.LoadAsync(result, token);
        }
    }
}