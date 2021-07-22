using HotChocolate.Resolvers;
using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.GraphQL.Observations;
using Sheaft.GraphQL.Recalls;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class RecallProductType : SheaftOutputType<RecallProduct>
    {
        protected override void Configure(IObjectTypeDescriptor<RecallProduct> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .Field(c => c.ProductId)
                .ID(nameof(Product))
                .Name("id");
            
            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");
            
            descriptor
                .Field(c => c.Name)
                .Name("name");
            
            descriptor
                .Field(c => c.Reference)
                .Name("reference");
            
            descriptor
                .Field(c => c.Conditioning)
                .Name("conditioning");
            
            descriptor
                .Field(c => c.QuantityPerUnit)
                .Name("quantityPerUnit");
            
            descriptor
                .Field(c => c.Picture)
                .Name("picture");
            
            descriptor
                .Field(c => c.Vat)
                .Name("vat");
            
            descriptor
                .Field(c => c.Unit)
                .Name("unit");
        }
    }
}