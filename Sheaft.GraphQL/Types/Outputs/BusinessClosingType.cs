using HotChocolate.Types;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class BusinessClosingType : ObjectType<BusinessClosing>
    {
        protected override void Configure(IObjectTypeDescriptor<BusinessClosing> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.ClosedFrom).Name("From");
            descriptor.Field(c => c.ClosedTo).Name("To");
            descriptor.Field(c => c.Reason);
        }
    }
}