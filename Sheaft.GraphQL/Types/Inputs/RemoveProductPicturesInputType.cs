using HotChocolate.Types;
using Sheaft.Mediatr.Product.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RemoveProductPicturesInputType : SheaftInputType<RemoveProductPicturesCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RemoveProductPicturesCommand> descriptor)
        {
            descriptor.Name("RemoveProductPicturesInput");
            descriptor.Field(c => c.PictureIds)
                .Type<NonNullType<ListType<IdType>>>();

            descriptor.Field(c => c.ProductId)
                .Name("id")
                .Type<NonNullType<IdType>>();
        }
    }
}