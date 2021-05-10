using System;
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
using Sheaft.GraphQL.Jobs;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.PickingOrders.Commands;
using Sheaft.Mediatr.PurchaseOrder.Commands;
using Sheaft.Mediatr.Transactions.Commands;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.GraphQL.Exports
{
    [ExtendObjectType(Name = "Mutation")]
    public class ExportMutations : SheaftMutation
    {
        public ExportMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("exportPickingOrders")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(JobType))]
        public async Task<Job> ExportPickingOrdersAsync(
            [GraphQLType(typeof(QueueExportPickingOrderInputType))] [GraphQLName("input")]
            QueueExportPickingOrderCommand input, [Service] ISheaftMediatr mediatr,
            JobsByIdBatchDataLoader jobsDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<QueueExportPickingOrderCommand, Guid>(mediatr, input, token);
            return await jobsDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("exportPurchaseOrders")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(JobType))]
        public async Task<Job> ExportPurchaseOrdersAsync(
            [GraphQLType(typeof(QueueExportPurchaseOrdersInputType))] [GraphQLName("input")]
            QueueExportPurchaseOrdersCommand input, [Service] ISheaftMediatr mediatr,
            JobsByIdBatchDataLoader jobsDataLoader, CancellationToken token)
        {
            var result =
                await ExecuteAsync<QueueExportPurchaseOrdersCommand, Guid>(mediatr, input, token);
            return await jobsDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("exportTransactions")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(JobType))]
        public async Task<Job> ExportTransactionsAsync(
            [GraphQLType(typeof(QueueExportTransactionsInputType))] [GraphQLName("input")]
            QueueExportTransactionsCommand input, [Service] ISheaftMediatr mediatr,
            JobsByIdBatchDataLoader jobsDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<QueueExportTransactionsCommand, Guid>(mediatr, input, token);
            return await jobsDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("exportUserData")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(JobType))]
        public async Task<Job> ExportUserDataAsync([Service] ISheaftMediatr mediatr, JobsByIdBatchDataLoader jobsDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<QueueExportUserDataCommand, Guid>(mediatr, new QueueExportUserDataCommand(CurrentUser), token);
            return await jobsDataLoader.LoadAsync(result, token);
        }
    }
}