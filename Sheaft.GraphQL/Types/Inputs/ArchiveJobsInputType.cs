using HotChocolate.Types;
using Sheaft.Mediatr.Job.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ArchiveJobsInputType : SheaftInputType<ArchiveJobsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ArchiveJobsCommand> descriptor)
        {
            descriptor.Name("ArchiveJobsInput");
            descriptor.Field(c => c.JobIds)
                .Name("Ids")
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}