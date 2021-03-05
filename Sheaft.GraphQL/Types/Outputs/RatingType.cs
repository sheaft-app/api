using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class RatingType : SheaftOutputType<RatingDto>
    {
        protected override void Configure(IObjectTypeDescriptor<RatingDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.Value);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Comment);

            descriptor.Field(c => c.User)
                .Type<NonNullType<UserType>>();
        }
    }
}
