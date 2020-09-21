using HotChocolate.Types;
using Sheaft.GraphQL.Enums;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
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
