﻿using System;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;

namespace Sheaft.Domain
{
    public class QuickOrderProduct
    {
        protected QuickOrderProduct() { }

        public QuickOrderProduct(CatalogProduct product, int quantity = 0)
        {
            CatalogProduct = product;
            Quantity = quantity;
        }

        public int? Quantity { get; private set; }
        public virtual CatalogProduct CatalogProduct { get; private set; }

        public void SetQuantity(int? quantity)
        {
            if (quantity.HasValue && quantity <= 0)
                throw new ValidationException(MessageKind.QuickOrder_ProductQuantity_CannotBe_LowerOrEqualThan, 0);

            Quantity = quantity;
        }
    }
}