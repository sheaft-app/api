using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Job.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class PauseJobsInputType : SheaftInputType<PauseJobsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<PauseJobsCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("PauseJobsInput");
            
            descriptor
                .Field(c => c.JobIds)
                .Name("ids")
                .ID(nameof(Job));
        }
    }
}