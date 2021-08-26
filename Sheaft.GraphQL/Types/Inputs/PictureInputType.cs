using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ProductPictureInputType : SheaftInputType<PictureInputDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PictureInputDto> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor.Name("ProductPictureInput");

            descriptor
                .Field(c => c.Id)
                .ID(nameof(ProductPicture))
                .Name("id");
            
            descriptor
                .Field(c => c.Data)
                .Name("data");

            descriptor
                .Field(c => c.Position)
                .Name("position");
        }
    }
}