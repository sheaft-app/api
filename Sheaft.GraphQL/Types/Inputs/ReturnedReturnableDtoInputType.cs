using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class ReturnedReturnableDtoInputType : SheaftInputType<ReturnedReturnableDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ReturnedReturnableDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ReturnedReturnableInput");

            descriptor
                .Field(c => c.ReturnableId)
                .Name("id")
                .ID(nameof(Returnable));

            descriptor
                .Field(c => c.Quantity)
                .Name("quantity");
        }
    }
}