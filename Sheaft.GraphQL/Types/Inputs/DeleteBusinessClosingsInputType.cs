using HotChocolate.Types;
using Sheaft.Mediatr.BusinessClosing.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteBusinessClosingsInputType : SheaftInputType<DeleteBusinessClosingsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteBusinessClosingsCommand> descriptor)
        {
            descriptor.Name("DeleteBusinessClosingsInput");
            descriptor.Field(c => c.ClosingIds)
                .Name("ids")
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}