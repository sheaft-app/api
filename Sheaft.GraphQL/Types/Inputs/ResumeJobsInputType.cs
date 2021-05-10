using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.Job.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ResumeJobsInputType : SheaftInputType<ResumeJobsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ResumeJobsCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ResumeJobsInput");

            descriptor
                .Field(c => c.JobIds)
                .Name("ids")
                .ID(nameof(Job));
        }
    }
}