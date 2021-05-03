using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class SearchProducerDtoType : SheaftOutputType<ProducerDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProducerDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor
                .Field(c => c.Phone)
                .Name("phone");
            
            descriptor
                .Field(c => c.Picture)
                .Name("picture");

            descriptor
                .Field(c => c.Id)
                .Name("id")
                .Type<NonNullType<IdType>>();
            
            descriptor
                .Field(c => c.Tags)
                .Name("tags")
                .Type<ListType<SearchTagDtoType>>();

            descriptor
                .Field(c => c.Name)
                .Name("name")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Email)
                .Name("email")
                .Type<NonNullType<StringType>>();

            descriptor
                .Field(c => c.Address)
                .Name("address")
                .Type<NonNullType<AddressDtoType>>();
        }
    }
}
