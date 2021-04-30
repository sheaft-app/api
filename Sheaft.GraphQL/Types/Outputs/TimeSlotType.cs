using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DeliveryHoursType : ObjectType<DeliveryHours>
    {
        protected override void Configure(IObjectTypeDescriptor<DeliveryHours> descriptor)
        {
            descriptor.Field(c => c.Day);
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
        }
    }
}
