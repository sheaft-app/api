using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class UserPositionType : SheaftOutputType<UserPositionDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UserPositionDto> descriptor)
        {
            descriptor.Field(c => c.Points);
            descriptor.Field(c => c.Position);
        }
    }
}
