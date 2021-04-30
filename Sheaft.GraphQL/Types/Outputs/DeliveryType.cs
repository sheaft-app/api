﻿using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Filters;
using Sheaft.GraphQL.Sorts;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DeliveryType : ObjectType<DeliveryDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DeliveryDto> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.Available);
            // descriptor.Field(c => c.Address)
            //     .Type<AddressType>();

            descriptor.Field(c => c.DeliveryHours)
                .Type<ListType<DeliveryHourType>>();
                
            descriptor.Field(c => c.Closings)
                .Type<ListType<DeliveryClosingType>>();
        }
    }
}
