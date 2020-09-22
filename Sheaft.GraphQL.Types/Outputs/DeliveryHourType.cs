﻿using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class DeliveryHourType : ObjectType<DeliveryHourDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DeliveryHourDto> descriptor)
        {
            descriptor.Field(c => c.ExpectedDeliveryDate);
            descriptor.Field(c => c.Day);
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
        }
    }
}