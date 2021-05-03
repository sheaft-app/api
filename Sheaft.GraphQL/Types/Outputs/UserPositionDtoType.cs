using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class UserPositionDtoType : SheaftOutputType<UserPositionDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserPositionDto> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .Field(c => c.Points)
                .Name("points");
                
            descriptor
                .Field(c => c.Position)
                .Name("position");
        }
    }
}
