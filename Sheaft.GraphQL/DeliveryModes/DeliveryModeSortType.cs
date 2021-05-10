using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.DeliveryModes
{
    public class DeliveryModeSortType : SortInputType<DeliveryModeDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<DeliveryModeDto> descriptor)
        {
            descriptor.Name("DeliveryModeSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.Name);
        }
    }
}
