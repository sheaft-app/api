using HotChocolate.Types;
using Sheaft.Mediatr.Product.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteProductsInputType : SheaftInputType<DeleteProductsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteProductsCommand> descriptor)
        {
            descriptor.Name("DeleteProductsInput");
            descriptor.Field(c => c.ProductIds)
                .Name("ids")
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}