using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Returnables;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ReturnableType : SheaftOutputType<Returnable>
    {
        protected override void Configure(IObjectTypeDescriptor<Returnable> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<ReturnablesByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));
            
            descriptor
                .Field(c => c.Name)
                .Name("name");
            
            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");
                
            descriptor
                .Field(c => c.UpdatedOn)
                .Name("updatedOn");
                
            descriptor
                .Field(c => c.Vat)
                .Name("vat");
                
            descriptor
                .Field(c => c.VatPrice)
                .Name("vatPrice");
                
            descriptor
                .Field(c => c.OnSalePrice)
                .Name("onSalePrice");
                
            descriptor
                .Field(c => c.WholeSalePrice)
                .Name("wholeSalePrice");
                
            descriptor
                .Field(c => c.VatPrice)
                .Name("vatPrice");
                
            descriptor
                .Field(c => c.Description)
                .Name("description");
        }
    }
}
