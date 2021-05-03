using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class PurchaseOrderUserDtoType : SheaftOutputType<PurchaseOrderUserDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PurchaseOrderUserDto> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .Field(c => c.Id)
                .Name("id")
                .Type<NonNullType<IdType>>();
            
            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();
            
            descriptor
                .Field(c => c.Email)
                .Name("email")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Kind)
                .Name("kind");
            
            descriptor
                .Field(c => c.Phone)
                .Name("phone");
            
            descriptor
                .Field(c => c.Picture)
                .Name("picture");
            
            descriptor
                .Field(c => c.Address)
                .Name("address");
        }
    }
}