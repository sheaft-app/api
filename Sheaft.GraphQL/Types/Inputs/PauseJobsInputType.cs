using HotChocolate.Types;
using Sheaft.Mediatr.Job.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class PauseJobsInputType : SheaftInputType<PauseJobsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PauseJobsCommand> descriptor)
        {
            descriptor.Name("PauseJobsInput");
            descriptor.Field(c => c.JobIds)
                .Name("Ids")
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}