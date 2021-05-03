using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.BusinessClosing.Commands;
using Sheaft.Mediatr.Legal.Commands;

namespace Sheaft.GraphQL.Business
{
    [ExtendObjectType(Name = "Mutation")]
    public class BusinessMutations : SheaftMutation
    {
        public BusinessMutations(
            ISheaftMediatr mediator,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(mediator, currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("createBusinessLegals")]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLType(typeof(BusinessLegalType))]
        public async Task<BusinessLegal> CreateBusinessLegalsAsync(
            [GraphQLName("input")] CreateBusinessLegalCommand input,
            BusinessLegalsByIdBatchDataLoader legalsDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateBusinessLegalCommand, Guid>(input, token);
            return await legalsDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateBusinessLegals")]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLType(typeof(BusinessLegalType))]
        public async Task<BusinessLegal> UpdateBusinessLegalsAsync(
            [GraphQLName("input")] UpdateBusinessLegalCommand input,
            BusinessLegalsByIdBatchDataLoader legalsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await legalsDataLoader.LoadAsync(input.LegalId, token);
        }

        [GraphQLName("updateOrCreateBusinessClosing")]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLType(typeof(BusinessClosingType))]
        public async Task<BusinessClosing> UpdateOrCreateBusinessClosingAsync(
            [GraphQLName("input")] UpdateOrCreateBusinessClosingCommand input,
            BusinessClosingsByIdBatchDataLoader businessClosingsDataLoader, CancellationToken token)
        {
            var result =
                await ExecuteAsync<UpdateOrCreateBusinessClosingCommand, Guid>(input, token);
            return await businessClosingsDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateOrCreateBusinessClosings")]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLType(typeof(ListType<BusinessClosingType>))]
        public async Task<IEnumerable<BusinessClosing>> UpdateOrCreateBusinessClosingsAsync(
            [GraphQLName("input")] UpdateOrCreateBusinessClosingsCommand input,
            BusinessClosingsByIdBatchDataLoader businessClosingsDataLoader, CancellationToken token)
        {
            var result =
                await ExecuteAsync<UpdateOrCreateBusinessClosingsCommand, IEnumerable<Guid>>(input, token);
            return await businessClosingsDataLoader.LoadAsync(result.ToList(), token);
        }

        [GraphQLName("deleteBusinessClosings")]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        public async Task<bool> DeleteBusinessClosingsAsync([GraphQLName("input")] DeleteBusinessClosingsCommand input,
            CancellationToken token)
        {
            return await ExecuteAsync(input, token);
        }
    }
}