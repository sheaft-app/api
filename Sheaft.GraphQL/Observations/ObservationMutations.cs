using System;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.Observation.Commands;

namespace Sheaft.GraphQL.Observations
{
    [ExtendObjectType(Name = "Mutation")]
    public class ObservationMutations : SheaftMutation
    {
        public ObservationMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("createObservation")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(ObservationType))]
        public async Task<Observation> CreateObservationAsync(
            [GraphQLType(typeof(CreateObservationInputType))] [GraphQLName("input")]
            CreateObservationCommand input, [Service] ISheaftMediatr mediatr,
            ObservationsByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateObservationCommand, Guid>(mediatr, input, token);
            return await dataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateObservation")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(ObservationType))]
        public async Task<Observation> UpdateObservationAsync(
            [GraphQLType(typeof(UpdateObservationInputType))] [GraphQLName("input")]
            UpdateObservationCommand input, [Service] ISheaftMediatr mediatr,
            ObservationsByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await dataLoader.LoadAsync(input.ObservationId, token);
        }

        [GraphQLName("replyToObservation")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(ObservationType))]
        public async Task<Observation> ReplyToObservationAsync(
            [GraphQLType(typeof(ReplyToObservationInputType))] [GraphQLName("input")]
            ReplyToObservationCommand input, [Service] ISheaftMediatr mediatr,
            ObservationsByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await dataLoader.LoadAsync(input.ObservationId, token);
        }

        [GraphQLName("deleteObservation")]
        [Authorize(Policy = Policies.REGISTERED)]
        public async Task<bool> DeleteObservationAsync(
            [GraphQLType(typeof(DeleteObservationInputType))] [GraphQLName("input")]
            DeleteObservationCommand input, [Service] ISheaftMediatr mediatr,
            CancellationToken token)
        {
            return await ExecuteAsync(mediatr, input, token);
        }
    }
}