using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Product.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteProductsInputType : SheaftInputType<DeleteProductsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteProductsCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeleteProductsInput");
            
            descriptor
                .Field(c => c.ProductIds)
                .Name("ids")
                .ID(nameof(Product));
        }
    }
}