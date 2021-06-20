﻿using HotChocolate.Types;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DeliveryDtoType : SheaftOutputType<DeliveryDto>
    {
        protected override void Configure(IObjectTypeDescriptor<DeliveryDto> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("ExpectedDelivery");
            
            descriptor.Field(c => c.Id)
                .Name("id")
                .ID(nameof(DeliveryMode));
            
            descriptor
                .Field(c => c.Kind)
                .Name("kind");
                
            descriptor
                .Field(c => c.Name)
                .Name("name");
                
            descriptor
                .Field(c => c.Available)
                .Name("available");

            descriptor
                .Field(c => c.Address)
                .Name("address")
                .Type<AddressDtoType>();

            descriptor
                .Field(c => c.DeliveryHours)
                .Name("deliveryHours")
                .Type<ListType<DeliveryHourDtoType>>();
                
            descriptor
                .Field(c => c.Closings)
                .Name("closings")
                .Type<ListType<ClosingDtoType>>();
        }
    }
}
