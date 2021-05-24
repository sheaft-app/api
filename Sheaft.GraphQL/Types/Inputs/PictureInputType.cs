using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Mediatr.Product.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class PictureInputType : SheaftInputType<PictureInputDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PictureInputDto> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor.Name("PictureInput");

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