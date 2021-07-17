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
using Sheaft.GraphQL.Producers;
using Sheaft.GraphQL.Recalls;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class RecallType : SheaftOutputType<Recall>
    {
        protected override void Configure(IObjectTypeDescriptor<Recall> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) => 
                    ctx.DataLoader<RecallsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.Name)
                .Name("name");
            
            descriptor
                .Field(c => c.Status)
                .Name("status");
            
            descriptor
                .Field(c => c.Comment)
                .Name("comment");
            
            descriptor
                .Field(c => c.SaleStartedOn)
                .Name("saleStartedOn");
            
            descriptor
                .Field(c => c.SaleEndedOn)
                .Name("saleEndedOn");
            
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
                .Field(c => c.Products)
                .Name("products")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchObservationResolvers>(c => c.GetProducts(default, default, default, default))
                .Type<ListType<RecallProductType>>();
            
            descriptor
                .Field(c => c.Batches)
                .Name("batches")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchObservationResolvers>(c => c.GetBatches(default, default, default, default))
                .Type<ListType<BatchType>>();
        }

        private class BatchObservationResolvers
        {
            public async Task<IEnumerable<RecallProduct>> GetProducts(Recall recall, [ScopedService] QueryDbContext context,
                RecallProductsByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var observationProductIds = await context.Set<RecallProduct>()
                    .Where(ob => ob.RecallId == recall.Id)
                    .Select(cp => cp.Id)
                    .ToListAsync(token);

                return await dataLoader.LoadAsync(observationProductIds, token);
            }
            
            public async Task<IEnumerable<Batch>> GetBatches(Recall observation, [ScopedService] QueryDbContext context,
                BatchesByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var batchIds = await context.Set<RecallBatch>()
                    .Where(ob => ob.RecallId == observation.Id)
                    .Select(cp => cp.BatchId)
                    .ToListAsync(token);

                return await dataLoader.LoadAsync(batchIds, token);
            }
            
            public Task<Producer> GetProducer(Observation observation, 
                ProducersByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                return dataLoader.LoadAsync(observation.ProducerId, token);
            }
        }
    }
}