using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SearchProducersDeliveriesInputType : SheaftInputType<SearchProducersDeliveriesDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SearchProducersDeliveriesDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("SearchProducersDeliveriesInput");

            descriptor
                .Field(c => c.Kinds)
                .Name("kinds");

            descriptor
                .Field(c => c.Ids)
                .Name("ids")
                .ID(nameof(Producer));
        }
    }
}
