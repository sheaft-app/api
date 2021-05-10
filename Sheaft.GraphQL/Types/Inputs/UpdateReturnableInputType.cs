using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Mediatr.Returnable.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateReturnableInputType : SheaftInputType<UpdateReturnableCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateReturnableCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("UpdateReturnableInput");
            
            descriptor
                .Field(c => c.Description)
                .Name("description");
                
            descriptor
                .Field(c => c.Vat)
                .Name("vat");
                
            descriptor
                .Field(c => c.WholeSalePrice)
                .Name("wholeSalePrice");

            descriptor.Field(c => c.ReturnableId)
                .Name("id")
                .ID(nameof(Returnable));

            descriptor.Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();
        }
    }
}
