﻿using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Enums;

namespace Sheaft.GraphQL.Types
{
    public class OwnerType : ObjectType<OwnerDto>
    {
        protected override void Configure(IObjectTypeDescriptor<OwnerDto> descriptor)
        {
            descriptor.Field(c => c.FirstName);
            descriptor.Field(c => c.LastName);
            descriptor.Field(c => c.Email);
            descriptor.Field(c => c.BirthDate);

            descriptor.Field(c => c.CountryOfResidence)
                .Type<NonNullType<CountryIsoCodeEnumType>>();

            descriptor.Field(c => c.Nationality)
                .Type<NonNullType<CountryIsoCodeEnumType>>();

            descriptor.Field(c => c.Address)
                .Type<AddressType>();
        }
    }
}