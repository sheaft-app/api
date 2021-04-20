using HotChocolate.Types;
using Sheaft.Mediatr.Job.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CancelJobsInputType : SheaftInputType<CancelJobsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CancelJobsCommand> descriptor)
        {
            descriptor.Name("CancelJobsInput");
            descriptor.Field(c => c.JobIds)
                .Name("ids")
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}