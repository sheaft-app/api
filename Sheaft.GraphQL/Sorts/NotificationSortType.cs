using HotChocolate.Data.Sorting;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Sorts
{
    public class NotificationSortType : SortInputType<NotificationDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<NotificationDto> descriptor)
        {
            descriptor.Name("NotificationSort");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.CreatedOn);
        }
    }
}
