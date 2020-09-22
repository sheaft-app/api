﻿using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Filters;

namespace Sheaft.GraphQL.Types
{
    public class AgreementType : SheaftOutputType<AgreementDto>
    {
        protected override void Configure(IObjectTypeDescriptor<AgreementDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.Status);

            descriptor.Field(c => c.Store)
                .Type<NonNullType<BusinessProfileType>>();

            descriptor.Field(c => c.Delivery)
                .Type<NonNullType<AgreementDeliveryModeType>>();

            descriptor.Field(c => c.SelectedHours)
                .Type<ListType<TimeSlotType>>()
                .UseFiltering<TimeSlotFilterType>();
        }
    }
}