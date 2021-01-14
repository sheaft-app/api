using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types
{
    public class CardRegistrationType : ObjectType<CardRegistrationDto>
    {
        protected override void Configure(IObjectTypeDescriptor<CardRegistrationDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<IdType>();
            descriptor.Field(c => c.Identifier);
            descriptor.Field(c => c.AccessKey);
            descriptor.Field(c => c.PreRegistrationData);
            descriptor.Field(c => c.RegistrationData);
            descriptor.Field(c => c.Url);
            descriptor.Field(c => c.Status).Type<CardStatusEnumType>();
        }
    }
}
