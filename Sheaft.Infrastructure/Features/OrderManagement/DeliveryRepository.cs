﻿using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.OrderManagement;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure.OrderManagement;

internal class DeliveryRepository : Repository<Delivery, DeliveryId>, IDeliveryRepository
{
    public DeliveryRepository(IDbContext context)
        : base(context)
    {
    }

    public override Task<Result<Delivery>> Get(DeliveryId identifier, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var result = await Values
                .SingleOrDefaultAsync(e => e.Id == identifier, token);
            
            return result != null
                ? Result.Success(result)
                : Result.Failure<Delivery>(ErrorKind.NotFound, "order.not.found");
        });
    }
}