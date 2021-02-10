using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class RewardType : SheaftOutputType<RewardDto>
    {
        protected override void Configure(IObjectTypeDescriptor<RewardDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.Contact);
            descriptor.Field(c => c.Email);
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Url);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
        }
    }
}
