using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.GraphQL.Batches;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.Users;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class BatchObservationType : SheaftOutputType<BatchObservation>
    {
        protected override void Configure(IObjectTypeDescriptor<BatchObservation> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) => 
                    ctx.DataLoader<BatchObservationsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(c => c.Comment)
                .Name("comment");
            
            descriptor
                .Field(c => c.User)
                .Type<UserType>()
                .Name("user");
            
            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");
            
            descriptor
                .Field(c => c.UpdatedOn)
                .Name("updatedOn");
            
            descriptor
                .Field(c => c.VisibleToAll)
                .Name("visibleToAll");
            
            descriptor
                .Field(c => c.Replies)
                .Name("replies")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchObservationResolvers>(c => c.GetReplies(default, default, default, default))
                .Type<ListType<BatchObservationType>>();
        }

        private class BatchObservationResolvers
        {
            public async Task<IEnumerable<BatchObservation>> GetReplies(BatchObservation batchObservation, [ScopedService] QueryDbContext context,
                BatchObservationsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var batchObservationIds = await context.Set<BatchObservation>()
                    .Where(cp => cp.BatchId == batchObservation.BatchId && cp.ReplyToId == batchObservation.Id)
                    .Select(cp => cp.Id)
                    .ToListAsync(token);

                var results = await dataLoader.LoadAsync(batchObservationIds, token);
                return results.OrderBy(r => r.CreatedOn);
            }
        }
    }
}