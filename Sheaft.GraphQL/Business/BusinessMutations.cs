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
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.Business.Commands;
using Sheaft.Mediatr.Legal.Commands;

namespace Sheaft.GraphQL.Business
{
    [ExtendObjectType(Name = "Mutation")]
    public class BusinessMutations : SheaftMutation
    {
        public BusinessMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("createBusinessLegals")]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLType(typeof(BusinessLegalType))]
        public async Task<BusinessLegal> CreateBusinessLegalsAsync(
            [GraphQLType(typeof(CreateBusinessLegalInputType))] [GraphQLName("input")]
            CreateBusinessLegalCommand input, [Service] ISheaftMediatr mediatr,
            BusinessLegalsByIdBatchDataLoader legalsDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateBusinessLegalCommand, Guid>(mediatr, input, token);
            return await legalsDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateBusinessLegals")]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLType(typeof(BusinessLegalType))]
        public async Task<BusinessLegal> UpdateBusinessLegalsAsync(
            [GraphQLType(typeof(UpdateBusinessLegalsInputType))] [GraphQLName("input")]
            UpdateBusinessLegalCommand input, [Service] ISheaftMediatr mediatr,
            BusinessLegalsByIdBatchDataLoader legalsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await legalsDataLoader.LoadAsync(input.LegalId, token);
        }

        [GraphQLName("updateOrCreateBusinessClosing")]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLType(typeof(BusinessClosingType))]
        public async Task<BusinessClosing> UpdateOrCreateBusinessClosingAsync(
            [GraphQLType(typeof(UpdateOrCreateBusinessClosingInputType))] [GraphQLName("input")]
            UpdateOrCreateBusinessClosingCommand input, [Service] ISheaftMediatr mediatr,
            BusinessClosingsByIdBatchDataLoader businessClosingsDataLoader, CancellationToken token)
        {
            var result =
                await ExecuteAsync<UpdateOrCreateBusinessClosingCommand, Guid>(mediatr, input, token);
            return await businessClosingsDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateOrCreateBusinessClosings")]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLType(typeof(ListType<BusinessClosingType>))]
        public async Task<IEnumerable<BusinessClosing>> UpdateOrCreateBusinessClosingsAsync(
            [GraphQLType(typeof(UpdateOrCreateBusinessClosingsInputType))] [GraphQLName("input")]
            UpdateOrCreateBusinessClosingsCommand input, [Service] ISheaftMediatr mediatr,
            BusinessClosingsByIdBatchDataLoader businessClosingsDataLoader, CancellationToken token)
        {
            var result =
                await ExecuteAsync<UpdateOrCreateBusinessClosingsCommand, IEnumerable<Guid>>(mediatr, input, token);
            return await businessClosingsDataLoader.LoadAsync(result.ToList(), token);
        }

        [GraphQLName("deleteBusinessClosings")]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        public async Task<bool> DeleteBusinessClosingsAsync(
            [GraphQLType(typeof(DeleteBusinessClosingsCommand))] [GraphQLName("input")]
            DeleteBusinessClosingsCommand input, [Service] ISheaftMediatr mediatr,
            CancellationToken token)
        {
            return await ExecuteAsync(mediatr, input, token);
        }
    }
}