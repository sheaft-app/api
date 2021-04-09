using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class CardType : SheaftOutputType<CardDto>
    {
        protected override void Configure(IObjectTypeDescriptor<CardDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Name);
        }
    }
}