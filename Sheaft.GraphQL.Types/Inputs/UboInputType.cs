﻿using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class UboInputType : SheaftInputType<CreateUboInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateUboInput> descriptor)
        {
            descriptor.Field(c => c.FirstName);
            descriptor.Field(c => c.LastName);
            descriptor.Field(c => c.BirthDate);
            descriptor.Field(c => c.Nationality);

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressInputType>>();

            descriptor.Field(c => c.BirthPlace)
                .Type<NonNullType<BirthAddressInputType>>();
        }
    }
}
