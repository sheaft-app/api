using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class PictureInputType : SheaftInputType<PictureInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PictureInput> descriptor)
        {
            descriptor.Field(c => c.Original);
            descriptor.Field(c => c.Resized);
        }
    }
}