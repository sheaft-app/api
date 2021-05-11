using HotChocolate.Types;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ProductPriceType : SheaftOutputType<CatalogProduct>
    {
        protected override void Configure(IObjectTypeDescriptor<CatalogProduct> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ProductPrice");

            descriptor
                .Field("id")
                .Resolve(c => c.Parent<CatalogProduct>().Product.Id)
                .ID(nameof(Product));

            descriptor
                .Field("name")
                .Resolve(c => c.Parent<CatalogProduct>().Product.Name);
            
            descriptor
                .Field("reference")
                .Resolve(c => c.Parent<CatalogProduct>().Product.Reference);
            
            descriptor
                .Field(c => c.VatPricePerUnit)
                .Name("vatPricePerUnit");
                
            descriptor
                .Field(c => c.OnSalePricePerUnit)
                .Name("onSalePricePerUnit");
                
            descriptor
                .Field(c => c.WholeSalePricePerUnit)
                .Name("wholeSalePricePerUnit");
                
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
                .Field(c => c.CreatedOn)
                .Name("addedOn");
        }
    }
}