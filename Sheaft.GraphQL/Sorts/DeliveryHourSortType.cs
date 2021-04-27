using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class DeliveryHourSortType : SortInputType<DeliveryHourDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<DeliveryHourDto> descriptor)
        {
            descriptor.Name("DeliveryHourSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Day);
            descriptor.Field(c => c.ExpectedDeliveryDate);
        }
    }
}
