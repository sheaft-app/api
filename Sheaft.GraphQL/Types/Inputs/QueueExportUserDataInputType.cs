using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class QueueExportUserDataInputType : SheaftInputType<QueueExportUserDataCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<QueueExportUserDataCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ExportUserDataInput");
            
            descriptor
                .Field(c => c.UserId)
                .Name("id")
                .ID(nameof(User));
        }
    }
}