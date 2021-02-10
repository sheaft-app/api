using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SearchProducersDeliveriesInputType : SheaftInputType<SearchProducersDeliveriesInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SearchProducersDeliveriesInput> descriptor)
        {
            descriptor.Field(c => c.Kinds)
                .Type<NonNullType<ListType<DeliveryKindEnumType>>>();

            descriptor.Field(c => c.Ids)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}
