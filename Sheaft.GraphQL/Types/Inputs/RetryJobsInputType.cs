using HotChocolate.Types;
using Sheaft.Mediatr.Job.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RetryJobsInputType : SheaftInputType<RetryJobsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RetryJobsCommand> descriptor)
        {
            descriptor.Name("RetryJobsInput");
            descriptor.Field(c => c.JobIds)
                .Name("Ids")
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}