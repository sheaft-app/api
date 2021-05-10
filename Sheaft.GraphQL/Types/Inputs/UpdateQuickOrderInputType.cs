using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Mediatr.QuickOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateQuickOrderInputType : SheaftInputType<UpdateQuickOrderCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateQuickOrderCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateQuickOrderInput");
            
            descriptor
                .Field(c => c.Description)
                .Name("description");

            descriptor
                .Field(c => c.QuickOrderId)
                .Name("id")
                .ID(nameof(QuickOrder));

            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();
            
            descriptor
                .Field(c => c.Products)
                .Name("products")
                .Type<ListType<ResourceIdQuantityInputType>>();
        }
    }
}
