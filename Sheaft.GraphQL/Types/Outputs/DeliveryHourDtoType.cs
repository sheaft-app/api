using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DeliveryHourDtoType : SheaftOutputType<DeliveryHourDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DeliveryHourDto> descriptor)
        {
            descriptor
                .Field(c => c.ExpectedDeliveryDate)
                .Name("expectedDeliveryDate");
            
            descriptor
                .Field(c => c.Day)
                .Name("day");

            descriptor
                .Field(c => c.From)
                .Name("from");

            descriptor
                .Field(c => c.To)
                .Name("to");
        }
    }
}