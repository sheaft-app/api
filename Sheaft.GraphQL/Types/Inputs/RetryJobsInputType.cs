using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Job.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class RetryJobsInputType : SheaftInputType<RetryJobsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RetryJobsCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("RetryJobsInput");
            
            descriptor
                .Field(c => c.JobIds)
                .Name("ids")
                .ID(nameof(Job));
        }
    }
}