using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AddOrUpdateProductsToCatalogInputType : SheaftInputType<AddOrUpdateProductsToCatalogDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AddOrUpdateProductsToCatalogDto> descriptor)
        {
            descriptor.Name("AddOrUpdateProductsToCatalogInput");
            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<UpdateOrCreateCatalogPriceInputType>>>();

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}