using HotChocolate.Types;
using Sheaft.Mediatr.Job.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ResumeJobsInputType : SheaftInputType<ResumeJobsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ResumeJobsCommand> descriptor)
        {
            descriptor.Name("ResumeJobsInput");
            descriptor.Field(c => c.JobIds)
                .Name("ids")
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}