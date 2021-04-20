using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Product.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class AddPictureToProductInputType : SheaftInputType<AddPictureToProductCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<AddPictureToProductCommand> descriptor)
        {
            descriptor.Name("AddPictureToProductInput");
            descriptor.Field(c => c.ProductId)
                .Name("id")
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Picture);
        }
    }
}