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
using Sheaft.GraphQL.Producers;
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
                .Name("user")
                .ResolveWith<BatchObservationResolvers>(c => c.GetUser(default, default, default));
            
            descriptor
                .Field(c => c.Producer)
                .Type<ProducerType>()
                .Name("producer")
                .ResolveWith<BatchObservationResolvers>(c => c.GetProducer(default, default, default));
            
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
                .Field(c => c.RepliesCount)
                .Name("repliesCount");
            
            descriptor
                .Field(c => c.BatchesCount)
                .Name("batchesCount");
            
            descriptor
                .Field(c => c.ProductsCount)
                .Name("productsCount");
            
            descriptor
                .Field(c => c.Replies)
                .Name("replies")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchObservationResolvers>(c => c.GetReplies(default, default, default, default))
                .Type<ListType<ObservationType>>();
            
            descriptor
                .Field(c => c.Products)
                .Name("products")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchObservationResolvers>(c => c.GetProducts(default, default, default, default))
                .Type<ListType<ObservationProductType>>();
            
            descriptor
                .Field(c => c.Batches)
                .Name("batches")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchObservationResolvers>(c => c.GetBatches(default, default, default, default))
                .Type<ListType<BatchType>>();
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
            
            public async Task<IEnumerable<ObservationProduct>> GetProducts(Observation observation, [ScopedService] QueryDbContext context,
                ObservationProductsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var observationProductIds = await context.Set<ObservationProduct>()
                    .Where(ob => ob.ObservationId == observation.Id)
                    .Select(cp => cp.Id)
                    .ToListAsync(token);

                return await dataLoader.LoadAsync(observationProductIds, token);
            }
            
            public async Task<IEnumerable<Batch>> GetBatches(Observation observation, [ScopedService] QueryDbContext context,
                BatchesByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var batchIds = await context.Set<ObservationBatch>()
                    .Where(ob => ob.ObservationId == observation.Id)
                    .Select(cp => cp.BatchId)
                    .ToListAsync(token);

                return await dataLoader.LoadAsync(batchIds, token);
            }
            
            public Task<Producer> GetProducer(Observation observation, 
                ProducersByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                return dataLoader.LoadAsync(observation.ProducerId, token);
            }
            
            public Task<User> GetUser(Observation observation, 
                UsersByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                return dataLoader.LoadAsync(observation.UserId, token);
            }
        }
    }
}