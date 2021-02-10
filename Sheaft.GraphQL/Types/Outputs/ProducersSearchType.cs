using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ProducersSearchType : SheaftOutputType<ProducersSearchDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProducersSearchDto> descriptor)
        {
            descriptor.Field(c => c.Count);
            descriptor.Field(c => c.Producers).Type<ListType<SearchProducerType>>();
        }
    }
}
