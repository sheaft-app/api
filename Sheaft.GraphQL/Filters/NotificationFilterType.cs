using HotChocolate.Data.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class NotificationFilterType : FilterInputType<NotificationDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<NotificationDto> descriptor)
        {
            descriptor.Name("NotificationFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Field(c => c.Id);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Unread);
        }
    }
}
