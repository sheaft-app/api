﻿using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class RegisterConsumerInputType : SheaftInputType<RegisterConsumerInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<RegisterConsumerInput> descriptor)
        {
            descriptor.Field(c => c.Anonymous);
            descriptor.Field(c => c.Phone);
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.SponsoringCode);

            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();

            descriptor.Field(c => c.Email)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.FirstName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.LastName)
                .Type<NonNullType<StringType>>();

            descriptor.Field(c => c.DepartmentId)
                .Type<IdType>();
        }
    }
}