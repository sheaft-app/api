﻿using HotChocolate.Types;
using Sheaft.Application.Common.Models.Inputs;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class SetProductsSearchabilityInputType : SheaftInputType<SetProductsSearchabilityInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<SetProductsSearchabilityInput> descriptor)
        {
            descriptor.Field(c => c.VisibleToConsumers);
            descriptor.Field(c => c.VisibleToStores);

            descriptor.Field(c => c.Ids)
                .Type<NonNullType<ListType<IdType>>>();
        }
    }
}