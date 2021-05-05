using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ProducersSearchDtoType : SheaftOutputType<ProducersSearchDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProducersSearchDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ProducersSearch");
            
            descriptor
                .Field(c => c.Count)
                .Name("count");
            
            descriptor
                .Field(c => c.Producers)
                .Name("producers")
                .Type<ListType<ProducerType>>();
        }
    }
}
