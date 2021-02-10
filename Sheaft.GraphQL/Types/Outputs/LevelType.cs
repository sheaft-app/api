using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.GraphQL.Filters;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class LevelType : SheaftOutputType<LevelDto>
    {
        protected override void Configure(IObjectTypeDescriptor<LevelDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Number);
            descriptor.Field(c => c.Name).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.RequiredPoints);

            descriptor.Field(c => c.Rewards)
                .Type<ListType<RewardType>>()
                .UseFiltering<RewardFilterType>();
        }
    }
}
