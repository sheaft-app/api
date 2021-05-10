using HotChocolate.Types;
using Sheaft.Domain;
using Sheaft.Mediatr.BusinessClosing.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class DeleteBusinessClosingsInputType : SheaftInputType<DeleteBusinessClosingsCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteBusinessClosingsCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeleteBusinessClosingsInput");
            
            descriptor
                .Field(c => c.ClosingIds)
                .Name("ids")
                .ID(nameof(BusinessClosing));
        }
    }
}