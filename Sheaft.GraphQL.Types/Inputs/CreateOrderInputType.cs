﻿using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class CreateOrderInputType : SheaftInputType<CreateOrderInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateOrderInput> descriptor)
        {
            descriptor.Field(c => c.Donation);

            descriptor.Field(c => c.ProducersExpectedDeliveries)
                .Type<ListType<ProducerExpectedDeliveryInputType>>();

            descriptor.Field(c => c.Products)
                .Type<NonNullType<ListType<ProductQuantityInputType>>>();
        }
    }
}
