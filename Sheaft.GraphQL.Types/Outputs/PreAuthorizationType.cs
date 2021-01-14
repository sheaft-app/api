using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types
{
    public class PreAuthorizationType : ObjectType<PreAuthorizationDto>
    {
        protected override void Configure(IObjectTypeDescriptor<PreAuthorizationDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<IdType>();
            descriptor.Field(c => c.Status).Type<PreAuthorizationStatusEnumType>();
            descriptor.Field(c => c.PaymentStatus).Type<PaymentStatusEnumType>();
            descriptor.Field(c => c.SecureModeRedirectURL);
        }
    }
}
