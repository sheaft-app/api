using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SearchProducersDeliveriesInputType : SheaftInputType<SearchProducersDeliveriesDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SearchProducersDeliveriesDto> descriptor)
        {
            descriptor.Name("SearchProducersDeliveriesInput");
            descriptor.Field(c => c.Kinds);

            descriptor.Field(c => c.Ids)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}
