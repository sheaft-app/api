using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RemoveProductsFromCatalogInputType : SheaftInputType<RemoveProductsFromCatalogDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RemoveProductsFromCatalogDto> descriptor)
        {
            descriptor.Name("RemoveProductsFromCatalogInput");
            descriptor.Field(c => c.ProductIds)
                .Type<NonNullType<ListType<IdType>>>();

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
        }
    }
}