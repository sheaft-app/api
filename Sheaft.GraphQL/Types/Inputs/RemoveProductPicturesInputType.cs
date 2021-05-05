using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Product.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RemoveProductPicturesInputType : SheaftInputType<RemoveProductPicturesCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RemoveProductPicturesCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("RemoveProductPicturesInput");
            
            descriptor
                .Field(c => c.PictureIds)
                .Name("pictureIds")
                .ID(nameof(ProductPicture));

            descriptor
                .Field(c => c.ProductId)
                .Name("id")
                .ID(nameof(Product));
        }
    }
}