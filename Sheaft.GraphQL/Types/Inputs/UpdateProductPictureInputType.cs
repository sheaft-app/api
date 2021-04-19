using HotChocolate.Types;
using Sheaft.Mediatr.Product.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateProductPictureInputType : SheaftInputType<UpdateProductPreviewCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateProductPreviewCommand> descriptor)
        {
            descriptor.Name("UpdateProductPictureInput");
            descriptor.Field(c => c.Picture)
                .Type<PictureSourceInputType>();

            descriptor.Field(c => c.ProductId)
                .Name("Id")
                .Type<NonNullType<IdType>>();
        }
    }
}