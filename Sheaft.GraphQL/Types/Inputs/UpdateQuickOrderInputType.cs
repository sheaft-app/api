using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.QuickOrder.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateQuickOrderInputType : SheaftInputType<UpdateQuickOrderCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateQuickOrderCommand> descriptor)
        {
            descriptor.Name("UpdateQuickOrderInput");
            descriptor.Field(c => c.Description);

            descriptor.Field(c => c.QuickOrderId)
                .Name("Id")
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();
            
            descriptor.Field(c => c.Products)
                .Type<ListType<ResourceIdQuantityInputType>>();
        }
    }
}
