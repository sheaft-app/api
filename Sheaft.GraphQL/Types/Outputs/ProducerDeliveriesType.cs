﻿using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.GraphQL.Filters;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ProducerDeliveriesType : SheaftOutputType<ProducerDeliveriesDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProducerDeliveriesDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Deliveries)
                .Type<ListType<DeliveryType>>()
                .UseFiltering<DeliveryFilterType>();

            descriptor.Field(c => c.Name).Type<NonNullType<StringType>>();
        }
    }
}