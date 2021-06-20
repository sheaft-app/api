using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Models;
using Sheaft.GraphQL.Agreements;
using Sheaft.GraphQL.DeliveryModes;
using Sheaft.GraphQL.Producers;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class AvailableDeliveryBatchDtoType : SheaftOutputType<AvailableDeliveryBatchDto>
    {
        protected override void Configure(IObjectTypeDescriptor<AvailableDeliveryBatchDto> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor
                .Field(c => c.Day)
                .Name("day");
            
            descriptor
                .Field(c => c.From)
                .Name("from");
            
            descriptor
                .Field(c => c.To)
                .Name("to");
            
            descriptor
                .Field(c => c.ExpectedDeliveryDate)
                .Name("expectedDeliveryDate");
            
            descriptor
                .Field(c => c.PurchaseOrdersCount)
                .Name("purchaseOrdersCount");
            
            descriptor
                .Field(c => c.ClientsCount)
                .Name("clientsCount");
            
            descriptor
                .Field(c => c.Clients)
                .Name("clients")
                .Type<ListType<AvailableClientDeliveryDtoType>>();
        }
    }
}