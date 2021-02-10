using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class JobType : SheaftOutputType<JobDto>
    {
        protected override void Configure(IObjectTypeDescriptor<JobDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.Archived);
            descriptor.Field(c => c.Retried);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.StartedOn);
            descriptor.Field(c => c.CompletedOn);
            descriptor.Field(c => c.Message);
            descriptor.Field(c => c.File);

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.User)
                .Type<NonNullType<UserProfileType>>();
        }
    }
}
