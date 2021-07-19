using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Deliveries;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DeliveryReturnableType : SheaftOutputType<DeliveryReturnable>
    {
        protected override void Configure(IObjectTypeDescriptor<DeliveryReturnable> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<DeliveryReturnablesByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.ReturnableId)
                .ID(nameof(Returnable))
                .Name("returnableId");
            
            descriptor
                .Field(c => c.Quantity)
                .Name("quantity");
            
                
            descriptor
                .Field(c => c.Vat)
                .Name("vat");
            
            descriptor
                .Field(c => c.UnitOnSalePrice)
                .Name("unitOnSalePrice");
                
            descriptor
                .Field(c => c.UnitVatPrice)
                .Name("unitVatPrice");
                
            descriptor
                .Field(c => c.UnitWholeSalePrice)
                .Name("unitWholeSalePrice");
                
            descriptor
                .Field(c => c.UnitVatPrice)
                .Name("unitVatPrice");
            
            descriptor
                .Field(c => c.TotalWholeSalePrice)
                .Name("totalWholeSalePrice");
                
            descriptor
                .Field(c => c.TotalVatPrice)
                .Name("totalVatPrice");
                
            descriptor
                .Field(c => c.TotalOnSalePrice)
                .Name("totalOnSalePrice");
            
            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();
        }
    }
}