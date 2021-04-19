﻿using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Legal.Commands;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class CreateConsumerLegalsInputType : SheaftInputType<CreateConsumerLegalCommand>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateConsumerLegalCommand> descriptor)
        {
            descriptor.Name("CreateConsumerLegalInput");
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
