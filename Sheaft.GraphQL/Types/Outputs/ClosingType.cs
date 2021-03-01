using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ClosingType : ObjectType<ClosingDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ClosingDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
            descriptor.Field(c => c.Reason);
        }
    }
}