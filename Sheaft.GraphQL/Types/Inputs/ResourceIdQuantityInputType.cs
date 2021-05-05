using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ResourceIdQuantityInputType : SheaftInputType<ResourceIdQuantityInputDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ResourceIdQuantityInputDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ResourceIdQuantityInput");
            
            descriptor
                .Field(c => c.Quantity)
                .Name("quantity");

            descriptor
                .Field(c => c.Id)
                .Name("id")
                .ID(nameof(Product));
        }
    }
}
