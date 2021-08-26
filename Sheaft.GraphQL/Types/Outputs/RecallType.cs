using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Batches;
using Sheaft.GraphQL.Producers;
using Sheaft.GraphQL.Recalls;
using Sheaft.GraphQL.Users;
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
                .Field(c => c.ProductsCount)
                .Name("productsCount");
            
            descriptor
                .Field(c => c.Products)
                .Name("products")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchObservationResolvers>(c => c.GetProducts(default, default, default, default))
                .Type<ListType<RecallProductType>>();
            
            descriptor
                .Field(c => c.BatchesCount)
                .Name("batchesCount");
            
            descriptor
                .Field(c => c.Batches)
                .Name("batches")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchObservationResolvers>(c => c.GetBatches(default, default, default, default))
                .Type<ListType<BatchType>>();
            
            descriptor
                .Field(c => c.ClientsCount)
                .Authorize(Policies.PRODUCER)
                .Name("clientsCount");
            
            descriptor
                .Field(c => c.Clients)
                .Name("clients")
                .Authorize(Policies.PRODUCER)
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchObservationResolvers>(c => c.GetClients(default, default, default, default))
                .Type<ListType<UserType>>();

            descriptor
                .Field("userAffected")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<BatchObservationResolvers>(c => c.UserIsAffected(default, default, default, default))
                .Type<BooleanType>();
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

            public async Task<bool> UserIsAffected(Recall recall, [ScopedService] QueryDbContext context,
                [Service] ICurrentUserService currentUserService, CancellationToken token)
            {
                var currentUser = currentUserService.GetCurrentUserInfo();
                if (!currentUser.Succeeded)
                    return false;
                
                return await context.Set<RecallClient>().AnyAsync(rc => rc.ClientId == currentUser.Data.Id && rc.RecallId == recall.Id, token);
            }

            public async Task<IEnumerable<Batch>> GetBatches(Recall recall, [ScopedService] QueryDbContext context,
                BatchesByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var batchIds = await context.Set<RecallBatch>()
                    .Where(ob => ob.RecallId == recall.Id)
                    .Select(cp => cp.BatchId)
                    .ToListAsync(token);

                return await dataLoader.LoadAsync(batchIds, token);
            }
            
            public async Task<IEnumerable<User>> GetClients(Recall recall, [ScopedService] QueryDbContext context,
                UsersByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                var clientIds = await context.Set<RecallClient>()
                    .Where(ob => ob.RecallId == recall.Id)
                    .Select(cp => cp.ClientId)
                    .ToListAsync(token);

                return await dataLoader.LoadAsync(clientIds, token);
            }
            
            public Task<Producer> GetProducer(Recall recall, 
                ProducersByIdBatchDataLoader dataLoader, CancellationToken token)
            {
                return dataLoader.LoadAsync(recall.ProducerId, token);
            }
        }
    }
}