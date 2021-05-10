using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Mediatr.Product.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AddPictureToProductInputType : SheaftInputType<AddPictureToProductCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AddPictureToProductCommand> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor.Name("AddPictureToProductInput");
            
            descriptor
                .Field(c => c.ProductId)
                .Name("id")
                .ID(nameof(Product));

            descriptor
                .Field(c => c.Picture)
                .Name("picture");
        }
    }
}