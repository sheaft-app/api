using HotChocolate.Types.Filters;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Filters
{
    public class NotificationFilterType : FilterInputType<NotificationDto>
    {
        protected override void Configure(IFilterInputTypeDescriptor<NotificationDto> descriptor)
        {
            descriptor.Name("NotificationFilter");
            descriptor.BindFieldsExplicitly();
            descriptor.Filter(c => c.Id).AllowEquals();
            descriptor.Filter(c => c.Kind).AllowIn();
            descriptor.Filter(c => c.Unread).AllowEquals();
        }
    }
}
