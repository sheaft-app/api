﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.GraphQL.Agreements;
using Sheaft.GraphQL.DeliveryModes;
using Sheaft.GraphQL.Producers;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Types.Outputs
{
    public class DeliveryModeType : SheaftOutputType<DeliveryMode>
    {
        protected override void Configure(IObjectTypeDescriptor<DeliveryMode> descriptor)
        {
            base.Configure(descriptor);

            descriptor.Name("DeliveryMode");

            descriptor
                .ImplementsNode()
                .IdField(c => c.Id)
                .ResolveNode((ctx, id) =>
                    ctx.DataLoader<DeliveryModesByIdBatchDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(c => c.CreatedOn)
                .Name("createdOn");

            descriptor
                .Field(c => c.UpdatedOn)
                .Name("updatedOn");

            descriptor
                .Field(c => c.LockOrderHoursBeforeDelivery)
                .Name("lockOrderHoursBeforeDelivery");

            descriptor
                .Field(c => c.Kind)
                .Name("kind");

            descriptor
                .Field(c => c.Name)
                .Name("name");

            descriptor
                .Field(c => c.MaxPurchaseOrdersPerTimeSlot)
                .Name("maxPurchaseOrdersPerTimeSlot");

            descriptor
                .Field(c => c.Available)
                .Name("available");

            descriptor
                .Field(c => c.AutoAcceptRelatedPurchaseOrder)
                .Name("autoAcceptRelatedPurchaseOrder");

            descriptor
                .Field(c => c.AutoCompleteRelatedPurchaseOrder)
                .Name("autoCompleteRelatedPurchaseOrder");

            descriptor
                .Field(c => c.Description)
                .Name("description");
            
            descriptor
                .Field(c => c.DeliveryFeesWholeSalePrice)
                .Name("deliveryFeesWholeSalePrice");
            
            descriptor
                .Field(c => c.DeliveryFeesVatPrice)
                .Name("deliveryFeesVatPrice");
            
            descriptor
                .Field(c => c.DeliveryFeesOnSalePrice)
                .Name("deliveryFeesOnSalePrice");
            
            descriptor
                .Field(c => c.DeliveryFeesMinPurchaseOrdersAmount)
                .Name("deliveryFeesMinPurchaseOrdersAmount");
            
            descriptor
                .Field(c => c.ApplyDeliveryFeesWhen)
                .Name("applyDeliveryFeesWhen");
            
            descriptor
                .Field(c => c.AcceptPurchaseOrdersWithAmountGreaterThan)
                .Name("acceptPurchaseOrdersWithAmountGreaterThan");
            
            descriptor
                .Field(c => c.DeliveryHoursCount)
                .Name("deliveryHoursCount");
            
            descriptor
                .Field(c => c.ClosingsCount)
                .Name("closingsCount");

            descriptor
                .Field(c => c.Address)
                .Name("address")
                .Type<DeliveryAddressType>();

            descriptor
                .Field(c => c.Producer)
                .Name("producer")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<DeliveryResolvers>(c => c.GetProducer(default, default, default))
                .Type<NonNullType<ProducerType>>();

            descriptor
                .Field(c => c.DeliveryHours)
                .Name("deliveryHours")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<DeliveryResolvers>(c => c.GetDeliveryHours(default, default, default, default))
                .Type<ListType<DeliveryHoursType>>();

            descriptor
                .Field(c => c.Closings)
                .Name("closings")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<DeliveryResolvers>(c => c.GetDeliveryClosings(default, default, default, default))
                .Type<ListType<DeliveryClosingType>>();

            descriptor
                .Field(c => c.Agreements)
                .Name("agreements")
                .UseDbContext<QueryDbContext>()
                .ResolveWith<DeliveryResolvers>(c => c.GetAgreements(default, default, default, default))
                .Type<ListType<AgreementType>>();
        }

        private class DeliveryResolvers
        {
            public async Task<IEnumerable<Agreement>> GetAgreements(DeliveryMode delivery,
                [ScopedService] QueryDbContext context, AgreementsByIdBatchDataLoader agreementsDataLoader,
                CancellationToken token)
            {
                var agreementsId = await context.Set<Agreement>()
                    .Where(d => d.DeliveryModeId == delivery.Id && d.Status == AgreementStatus.Accepted)
                    .Select(d => d.Id)
                    .ToListAsync(token);

                return await agreementsDataLoader.LoadAsync(agreementsId, token);
            }

            public async Task<IEnumerable<DeliveryHours>> GetDeliveryHours(DeliveryMode delivery,
                [ScopedService] QueryDbContext context, DeliveryHoursByIdBatchDataLoader deliveryHoursDataLoader,
                CancellationToken token)
            {
                var deliveryHoursId = await context.Set<DeliveryHours>()
                    .Where(d => d.DeliveryModeId == delivery.Id)
                    .Select(d => d.Id)
                    .ToListAsync(token);

                return await deliveryHoursDataLoader.LoadAsync(deliveryHoursId, token);
            }

            public async Task<IEnumerable<DeliveryClosing>> GetDeliveryClosings(DeliveryMode delivery,
                [ScopedService] QueryDbContext context, DeliveryClosingsByIdBatchDataLoader deliveryClosingsDataLoader,
                CancellationToken token)
            {
                var deliveryHoursId = await context.Set<DeliveryClosing>()
                    .Where(d => d.DeliveryModeId == delivery.Id && d.ClosedTo > DateTimeOffset.UtcNow)
                    .Select(d => d.Id)
                    .ToListAsync(token);

                return await deliveryClosingsDataLoader.LoadAsync(deliveryHoursId, token);
            }
            
            public Task<Producer> GetProducer(DeliveryMode delivery, ProducersByIdBatchDataLoader producersDataLoader,
                CancellationToken token)
            {
                return producersDataLoader.LoadAsync(delivery.ProducerId, token);
            }
        }
    }
}