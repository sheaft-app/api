using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Job.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CancelJobsInputType : SheaftInputType<CancelJobsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CancelJobsCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CancelJobsInput");
            
            descriptor
                .Field(c => c.JobIds)
                .Name("ids")
                .ID(nameof(Job));
        }
    }
}