using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ProfilePictureInputType : SheaftInputType<PictureInputDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PictureInputDto> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor.Name("ProfilePictureInput");

            descriptor
                .Field(c => c.Id)
                .ID(nameof(ProfilePicture))
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