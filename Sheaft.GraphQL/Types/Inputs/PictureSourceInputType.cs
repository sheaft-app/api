using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class PictureSourceInputType : SheaftInputType<PictureSourceDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PictureSourceDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("PictureInput");
            
            descriptor
                .Field(c => c.Original)
                .Name("original");
                
            descriptor
                .Field(c => c.Resized)
                .Name("resized");
        }
    }
}