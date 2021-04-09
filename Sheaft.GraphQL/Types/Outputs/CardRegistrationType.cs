using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class CardRegistrationType : ObjectType<CardRegistrationDto>
    {
        protected override void Configure(IObjectTypeDescriptor<CardRegistrationDto> descriptor)
        {
            descriptor.Field(c => c.Identifier);
            descriptor.Field(c => c.AccessKey);
            descriptor.Field(c => c.PreRegistrationData);
            descriptor.Field(c => c.Url);
        }
    }
}