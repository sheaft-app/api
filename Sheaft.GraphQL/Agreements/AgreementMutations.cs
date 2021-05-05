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
using Sheaft.Mediatr.Agreement.Commands;

namespace Sheaft.GraphQL.Agreements
{
    [ExtendObjectType(Name = "Mutation")]
    public class AgreementMutations : SheaftMutation
    {
        public AgreementMutations(
            ISheaftMediatr mediator,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(mediator, currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("createAgreement")]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLType(typeof(AgreementType))]
        public async Task<Agreement> CreateAgreementAsync(
            [GraphQLType(typeof(CreateAgreementInputType))] [GraphQLName("input")]
            CreateAgreementCommand input,
            AgreementsByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateAgreementCommand, Guid>(input, token);
            return await dataLoader.LoadAsync(result, token);
        }

        [GraphQLName("acceptAgreements")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ListType<AgreementType>))]
        public async Task<IEnumerable<Agreement>> AcceptAgreementsAsync(
            [GraphQLType(typeof(AcceptAgreementsInputType))] [GraphQLName("input")]
            AcceptAgreementsCommand input,
            AgreementsByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await dataLoader.LoadAsync(input.AgreementIds.ToList(), token);
        }

        [GraphQLName("assignCatalogToAgreement")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(AgreementType))]
        public async Task<Agreement> AssignCatalogToAgreementAsync(
            [GraphQLType(typeof(AssignCatalogToAgreementInputType))] [GraphQLName("input")]
            AssignCatalogToAgreementCommand input,
            AgreementsByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await dataLoader.LoadAsync(input.AgreementId, token);
        }

        [GraphQLName("cancelAgreements")]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLType(typeof(ListType<AgreementType>))]
        public async Task<IEnumerable<Agreement>> CancelAgreementsAsync(
            [GraphQLType(typeof(CancelAgreementsInputType))] [GraphQLName("input")]
            CancelAgreementsCommand input,
            AgreementsByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await dataLoader.LoadAsync(input.AgreementIds.ToList(), token);
        }

        [GraphQLName("refuseAgreements")]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [GraphQLType(typeof(ListType<AgreementType>))]
        public async Task<IEnumerable<Agreement>> RefuseAgreementsAsync(
            [GraphQLType(typeof(RefuseAgreementsInputType))] [GraphQLName("input")]
            RefuseAgreementsCommand input,
            AgreementsByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await dataLoader.LoadAsync(input.AgreementIds.ToList(), token);
        }
    }
}