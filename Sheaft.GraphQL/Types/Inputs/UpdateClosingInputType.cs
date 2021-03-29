﻿using HotChocolate.Types;
using Sheaft.Application.Models;

namespace Sheaft.GraphQL.Types.Inputs
{
    public class UpdateClosingInputType : SheaftInputType<UpdateOrCreateClosingDto>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateOrCreateClosingDto> descriptor)
        {
            descriptor.Name("UpdateClosingInput");
            descriptor.Field(c => c.Id)
                .Type<NonNullType<IdType>>();
            descriptor.Field(c => c.From);
            descriptor.Field(c => c.To);
            descriptor.Field(c => c.Reason);
        }
    }
}