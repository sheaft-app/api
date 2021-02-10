using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class BusinessLegalType : SheaftOutputType<BusinessLegalDto>
    {
        protected override void Configure(IObjectTypeDescriptor<BusinessLegalDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Validation);
            descriptor.Field(c => c.Email);
            descriptor.Field(c => c.Address).Type<AddressType>();
            descriptor.Field(c => c.UboDeclaration).Type<UboDeclarationType>();
            descriptor.Field(c => c.Owner).Type<OwnerType>();
        }
    }
}
