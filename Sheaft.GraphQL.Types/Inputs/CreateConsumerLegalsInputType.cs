﻿using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class CreateConsumerLegalsInputType : SheaftInputType<CreateConsumerLegalInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateConsumerLegalInput> descriptor)
        {
            descriptor.Field(c => c.FirstName);
            descriptor.Field(c => c.LastName);
            descriptor.Field(c => c.Email);
            descriptor.Field(c => c.BirthDate);
            descriptor.Field(c => c.Nationality);
            descriptor.Field(c => c.CountryOfResidence);

            descriptor.Field(c => c.UserId)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressInputType>>();
        }
    }
}