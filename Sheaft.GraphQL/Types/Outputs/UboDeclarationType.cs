﻿using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class UboDeclarationType : SheaftOutputType<DeclarationDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DeclarationDto> descriptor)
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