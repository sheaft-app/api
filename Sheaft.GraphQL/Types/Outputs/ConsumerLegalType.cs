﻿using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class ConsumerLegalType : SheaftOutputType<ConsumerLegalDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ConsumerLegalDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Validation);
            descriptor.Field(c => c.Owner).Type<OwnerType>();
        }
    }
}