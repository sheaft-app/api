﻿using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class RegisterStoreInputType : SheaftInputType<RegisterStoreInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RegisterStoreInput> descriptor)
        {
            descriptor.Field(c => c.OpenForNewBusiness);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.SponsoringCode);

            descriptor.Field(c => c.Address)
                .Type<NonNullType<FullAddressInputType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Name)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.Tags)
                .Type<ListType<IdType>>();

            descriptor.Field(c => c.OpeningHours)
                .Type<ListType<TimeSlotGroupInputType>>();

            descriptor.Field(c => c.Legals)
                .Type<CreateBusinessLegalsInputType>();
        }
    }
}
