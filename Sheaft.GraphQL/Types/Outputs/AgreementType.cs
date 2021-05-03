using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Agreements;
using Sheaft.GraphQL.Catalogs;
using Sheaft.GraphQL.DeliveryModes;
using Sheaft.GraphQL.Producers;
using Sheaft.GraphQL.Stores;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class AgreementType : SheaftOutputType<Agreement>
    {
        protected override void Configure(IObjectTypeDescriptor<Agreement> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<AgreementsByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(c => c.CreatedByKind)
                .Name("createdByKind");
            
            descriptor.Field(c => c.CreatedOn).Name("createdOn");
            descriptor.Field(c => c.UpdatedOn).Name("updatedOn");
            descriptor.Field(c => c.Status).Name("status");
            descriptor.Field(c => c.Reason).Name("reason");

            descriptor
                .Field(c => c.Store)
                .Name("store")
                .UseDbContext<AppDbContext>()
                .ResolveWith<AgreementResolvers>(c => c.GetStore(default!, default!, default))
                .Type<NonNullType<StoreType>>();
            
            descriptor
                .Field(c => c.Producer)
                .Name("producer")
                .UseDbContext<AppDbContext>()
                .ResolveWith<AgreementResolvers>(c => c.GetProducer(default!, default, default))
                .Type<NonNullType<ProducerType>>();

            descriptor
                .Field(c => c.Delivery)
                .Name("delivery")
                .UseDbContext<AppDbContext>()
                .ResolveWith<AgreementResolvers>(c => c.GetDelivery(default, default, default))
                .Type<DeliveryModeType>();
            
            descriptor
                .Field(c => c.Catalog)
                .Name("catalog")
                .UseDbContext<AppDbContext>()
                .ResolveWith<AgreementResolvers>(c => c.GetCatalog(default, default, default))
                .Type<CatalogType>();
        }

        private class AgreementResolvers
        {
            public Task<Store> GetStore(Agreement agreement, StoresByIdBatchDataLoader storesDataLoader, CancellationToken token)
            {
                return storesDataLoader.LoadAsync(agreement.StoreId, token);
            }
            
            public Task<DeliveryMode> GetDelivery(Agreement agreement, DeliveryModesByIdBatchDataLoader deliveriesDataLoader, CancellationToken token)
            {
                if (!agreement.DeliveryId.HasValue)
                    return null;
                
                return deliveriesDataLoader.LoadAsync(agreement.DeliveryId.Value, token);
            }
            
            public Task<Producer> GetProducer(Agreement agreement, ProducersByIdBatchDataLoader producersDataLoader, CancellationToken token)
            {
                return producersDataLoader.LoadAsync(agreement.ProducerId, token);
            }
            
            public Task<Catalog> GetCatalog(Agreement agreement, CatalogsByIdBatchDataLoader catalogsDataLoader, CancellationToken token)
            {
                return catalogsDataLoader.LoadAsync(agreement.CatalogId, token);
            }
        }
    }
}
