using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Filters;

namespace Sheaft.GraphQL.Types
{
    public class QuickOrderType : SheaftOutputType<QuickOrderDto>
    {
        protected override void Configure(IObjectTypeDescriptor<QuickOrderDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.IsDefault);
            descriptor.Field(c => c.Description);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.User)
                .Type<NonNullType<UserProfileType>>();

            descriptor.Field(c => c.Products)
                .Type<ListType<QuickOrderProductQuantityType>>()
                .UseFiltering<QuickOrderProductQuantityFilterType>();
        }
    }
}
