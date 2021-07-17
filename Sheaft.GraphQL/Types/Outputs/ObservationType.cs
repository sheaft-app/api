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
using Sheaft.GraphQL.Observations;
using Sheaft.GraphQL.Users;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ObservationType : SheaftOutputType<Observation>
    {
        protected override void Configure(IObjectTypeDescriptor<Observation> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) => 
                    ctx.DataLoader<ObservationsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

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
                .Type<ListType<ObservationType>>();
        }

        private class BatchObservationResolvers
        {
            public async Task<IEnumerable<Observation>> GetReplies(Observation observation, [ScopedService] QueryDbContext context,
                ObservationsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var batchObservationIds = await context.Set<Observation>()
                    .Where(cp => cp.ReplyToId == observation.Id)
                    .Select(cp => cp.Id)
                    .ToListAsync(token);

                var results = await dataLoader.LoadAsync(batchObservationIds, token);
                return results.OrderBy(r => r.CreatedOn);
            }
        }
    }
}