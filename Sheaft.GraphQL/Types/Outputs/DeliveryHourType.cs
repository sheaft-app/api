using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DeliveryHourType : ObjectType<DeliveryHourDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DeliveryHourDto> descriptor)
        {
            descriptor.Field(c => c.ExpectedDeliveryDate);
            descriptor.Field(c => c.Day);
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
        }
    }
}
