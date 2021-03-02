﻿using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.ViewModels
{
    public class PurchaseOrderShortViewModel
    {
        public Guid Id { get; set; }
        public string Reference { get; set; }
        public decimal TotalOnSalePrice { get; set; }
        public PurchaseOrderStatus Status { get; set; }
    }
}