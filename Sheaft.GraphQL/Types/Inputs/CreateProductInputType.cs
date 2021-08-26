using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Product.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateProductInputType : SheaftInputType<CreateProductCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateProductCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CreateProductInput");
            
            descriptor
                .Field(c => c.Reference)
                .Name("reference");
                
            descriptor
                .Field(c => c.Available)
                .Name("available");
                
            descriptor
                .Field(c => c.Description)
                .Name("description");
                
            descriptor
                .Field(c => c.QuantityPerUnit)
                .Name("quantityPerUnit");
                
            descriptor
                .Field(c => c.Unit)
                .Name("unit");
                
            descriptor
                .Field(c => c.Conditioning)
                .Name("conditioning");
                
            descriptor
                .Field(c => c.Vat)
                .Name("vat");
                
            descriptor
                .Field(c => c.Weight)
                .Name("weight");

            descriptor.Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();
            
            descriptor.Field(c => c.Catalogs)
                .Name("catalogs")
                .Type<ListType<CatalogPriceInputType>>();
            
            descriptor.Field(c => c.ReturnableId)
                .Name("returnableId")
                .ID(nameof(Returnable));
            
            descriptor.Field(c => c.Tags)
                .Name("tags")
                .ID(nameof(Tag));

            descriptor.Field(c => c.Pictures)
                .Name("pictures")
                .Type<ListType<ProductPictureInputType>>();

        }
    }
}
