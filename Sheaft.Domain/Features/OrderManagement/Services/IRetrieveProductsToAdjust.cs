﻿namespace Sheaft.Domain.OrderManagement;

public interface IRetrieveProductsToAdjust
{
    Task<Result<IEnumerable<DeliveryLine>>> Get(Delivery delivery, IEnumerable<ProductAdjustment> productAdjustments, CancellationToken token);
}