﻿using HotChocolate.Types;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.GraphQL.Types.Outputs
{

    public class ProducerSummaryType : SheaftOutputType<ProducerSummaryDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ProducerSummaryDto> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.Picture);
            descriptor.Field(c => c.Description);
            descriptor.Field(c => c.FirstName);
            descriptor.Field(c => c.LastName);
            descriptor.Field(c => c.Name);

            descriptor.Field(c => c.Address)
                .Type<NonNullType<AddressType>>();
        }
    }
}
