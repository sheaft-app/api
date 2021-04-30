using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DeliveryClosingType : ObjectType<DeliveryClosing>
    {
        protected override void Configure(IObjectTypeDescriptor<DeliveryClosing> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.ClosedFrom).Name("From");
            descriptor.Field(c => c.ClosedTo).Name("To");
            descriptor.Field(c => c.Reason);
        }
    }
}