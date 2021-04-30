using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DeliveryModeType : SheaftOutputType<DeliveryMode>
    {
        protected override void Configure(IObjectTypeDescriptor<DeliveryMode> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Field(c => c.Id).Type<NonNullType<IdType>>();
            descriptor.Field(c => c.CreatedOn);
            descriptor.Field(c => c.UpdatedOn);
            descriptor.Field(c => c.LockOrderHoursBeforeDelivery);
            descriptor.Field(c => c.Kind);
            descriptor.Field(c => c.Name);
            descriptor.Field(c => c.MaxPurchaseOrdersPerTimeSlot);
            descriptor.Field(c => c.Available);
            descriptor.Field(c => c.AutoAcceptRelatedPurchaseOrder);
            descriptor.Field(c => c.AutoCompleteRelatedPurchaseOrder);
            descriptor.Field(c => c.Description);

            // descriptor.Field(c => c.Address)
            //     .Type<DeliveryAddressType>();

            descriptor.Field(c => c.Producer)
                .Type<NonNullType<UserType>>();

            descriptor.Field("deliveryHours")
                .UseDbContext<AppDbContext>()
                .ResolveWith<DeliveryResolvers>(c => c.GetDeliveryHours(default, default, default))
                .Type<ListType<DeliveryHoursType>>();
            
            descriptor.Field(c => c.Closings)
                .Type<ListType<DeliveryClosingType>>();
        }
        
        private class DeliveryResolvers
        {
            public async Task<IEnumerable<DeliveryHours>> GetDeliveryHours(DeliveryMode delivery, [ScopedService] AppDbContext context, CancellationToken token)
            {
                return await context.Set<DeliveryHours>().Where(s => s.DeliveryModeId == delivery.Id).ToListAsync(token);
            }
        }
    }
}
