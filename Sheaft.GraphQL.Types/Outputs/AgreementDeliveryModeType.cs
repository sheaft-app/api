﻿using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class AgreementDeliveryModeType : SheaftOutputType<AgreementDeliveryModeDto>
    {
        protected override void Configure(IObjectTypeDescriptor<AgreementDeliveryModeDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.LockOrderHoursBeforeDelivery);
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Description);

            descriptor.Field(c => c.Address)
                .Type<AddressType>();

            descriptor.Field(c => c.Producer)
                .Type<NonNullType<BusinessProfileType>>();
        }
    }
}
