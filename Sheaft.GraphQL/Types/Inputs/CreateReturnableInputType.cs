using HotChocolate.Types;
using Sheaft.Mediatr.Returnable.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateReturnableInputType : SheaftInputType<CreateReturnableCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateReturnableCommand> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("CreateReturnableInput");
            
            descriptor
                .Field(c => c.Vat)
                .Name("vat");
                
            descriptor
                .Field(c => c.Description)
                .Name("description");
                
            descriptor
                .Field(c => c.WholeSalePrice)
                .Name("wholeSalePrice");

            descriptor.Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();
        }
    }
}
