using HotChocolate.Types;
using Sheaft.Domain.Enums;

namespace Sheaft.GraphQL.Enums
{
    public class ProcessStatusEnumType : EnumType<ProcessStatus>
    {
        protected override void Configure(IEnumTypeDescriptor<ProcessStatus> descriptor)
        {
            descriptor.Value(ProcessStatus.Cancelled).Name("CANCELLED");
            descriptor.Value(ProcessStatus.Done).Name("DONE");
            descriptor.Value(ProcessStatus.Expired).Name("EXPIRED");
            descriptor.Value(ProcessStatus.Failed).Name("FAILED");
            descriptor.Value(ProcessStatus.Paused).Name("PAUSED");
            descriptor.Value(ProcessStatus.Processing).Name("PROCESSING");
            descriptor.Value(ProcessStatus.Rollbacked).Name("ROLLBACKED");
            descriptor.Value(ProcessStatus.Waiting).Name("WAITING");
        }
    }
}
