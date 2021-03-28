using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateProductInputType : SheaftInputType<CreateProductDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateProductDto> descriptor)
        {
            descriptor.Name("CreateProductInput");
            descriptor.Field(c => c.Reference);
            descriptor.Field(c => c.Available);
            descriptor.Field(c => c.VisibleToStores);
            descriptor.Field(c => c.VisibleToConsumers);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.OriginalPicture);
            descriptor.Field(c => c.QuantityPerUnit);
            descriptor.Field(c => c.Unit);
            descriptor.Field(c => c.Conditioning);
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.Weight);

            descriptor.Field(c => c.ReturnableId)
                .Type<IdType>();
            
            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Picture)
                .Type<PictureSourceInputType>();

            descriptor.Field(c => c.Tags)
                .Type<NonNullType<ListType<IdType>>>();

            descriptor.Field(c => c.Closings)
                .Type<ListType<UpdateOrCreateClosingInputType>>();
            
            descriptor.Field(c => c.Prices)
                .Type<NonNullType<ListType<CatalogPriceInputType>>>();
        }
    }
}
