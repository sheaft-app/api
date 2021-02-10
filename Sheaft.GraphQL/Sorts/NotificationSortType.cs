using HotChocolate.Types.Sorting;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Sorts
{
    public class NotificationSortType : SortInputType<NotificationDto>
    {
        protected override void Configure(ISortInputTypeDescriptor<NotificationDto> descriptor)
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Sortable(c => c.CreatedOn);
        }
    }
}
