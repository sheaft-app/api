using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Product.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateProductPictureInputType : SheaftInputType<UpdateProductPreviewCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateProductPreviewCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateProductPictureInput");
            
            descriptor
                .Field(c => c.Picture)
                .Name("picture")
                .Type<PictureSourceInputType>();

            descriptor
                .Field(c => c.ProductId)
                .Name("id")
                .ID(nameof(Product));
        }
    }
}