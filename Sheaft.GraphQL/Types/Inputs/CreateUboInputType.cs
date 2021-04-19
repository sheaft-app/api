﻿using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Ubo.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateUboInputType : SheaftInputType<CreateUboCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateUboCommand> descriptor)
        {
            descriptor.Name("CreateUboInput");
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
