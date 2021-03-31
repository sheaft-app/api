using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateProductInputType : SheaftInputType<UpdateProductDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateProductDto> descriptor)
        {
            descriptor.Name("UpdateProductInput");
            descriptor.Field(c => c.VisibleToConsumers);
            descriptor.Field(c => c.VisibleToStores);
            descriptor.Field(c => c.Available);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.ReturnableId).Type<IdType>();
            descriptor.Field(c => c.OriginalPicture);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.QuantityPerUnit);
            descriptor.Field(c => c.Unit);
            descriptor.Field(c => c.Conditioning);
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.Weight);
            descriptor.Field(c => c.WholeSalePricePerUnit);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Reference)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Tags)
                .Type<NonNullType<ListType<IdType>>>();

            descriptor.Field(c => c.Closings)
                .Type<ListType<UpdateOrCreateClosingInputType>>();
        }
    }
}
