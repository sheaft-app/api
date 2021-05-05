using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Job.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ArchiveJobsInputType : SheaftInputType<ArchiveJobsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ArchiveJobsCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ArchiveJobsInput");
            
            descriptor
                .Field(c => c.JobIds)
                .Name("ids")
                .ID(nameof(Job));
        }
    }
}