using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class ProductInputType : SheaftInputType<CreateProductInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateProductInput> descriptor)
        {
            descriptor.Field(c => c.Available);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.ReturnableId).Type<IdType>();
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.QuantityPerUnit);
            descriptor.Field(c => c.Unit);
            descriptor.Field(c => c.Conditioning);
            descriptor.Field(c => c.Vat);
            descriptor.Field(c => c.Weight);
            descriptor.Field(c => c.WholeSalePricePerUnit);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Reference)
                .Type<StringType>();

            descriptor.Field(c => c.Tags)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}
