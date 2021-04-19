using HotChocolate.Types;
using Sheaft.Mediatr.User.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class QueueExportUserDataInputType : SheaftInputType<QueueExportUserDataCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<QueueExportUserDataCommand> descriptor)
        {
            descriptor.Name("ExportUserDataInput");
            descriptor.Field(c => c.UserId)
                .Name("Id")
                .Type<NonNullType<IdType>>();
        }
    }
}