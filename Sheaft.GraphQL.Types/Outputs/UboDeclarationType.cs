﻿using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types
{
    public class UboDeclarationType : SheaftOutputType<UboDeclarationDto>
    {
        protected override void Configure(IObjectTypeDescriptor<UboDeclarationDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Status);
            descriptor.Field(c => c.ReasonCode);
            descriptor.Field(c => c.ReasonMessage);

            descriptor.Field(c => c.Ubos)
                .Type<ListType<UboType>>();
        }
    }
}