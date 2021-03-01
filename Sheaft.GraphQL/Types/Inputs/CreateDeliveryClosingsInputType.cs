using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateDeliveryClosingsInputType : SheaftInputType<CreateDeliveryClosingsInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateDeliveryClosingsInput> descriptor)
        {
            descriptor.Field(c => c.DeliveryId)
                .Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Closings)
                .Type<NonNullType<ListType<ClosingInputType>>>();
        }
    }
}