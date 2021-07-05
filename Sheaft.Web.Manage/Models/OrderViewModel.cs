using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public DateTimeOffset? RemovedOn { get; set; }
        public OrderStatus Status { get; set; }
        public DonationKind DonationKind { get; set; }
        public string Reference { get; set; }
        public decimal TotalProductWholeSalePrice { get; set; }
        public decimal TotalProductVatPrice { get; set; }
        public decimal TotalProductOnSalePrice { get; set; }
        public decimal TotalReturnableOnSalePrice { get; set; }
        public decimal TotalReturnableWholeSalePrice { get; set; }
        public decimal TotalReturnableVatPrice { get; set; }
        public decimal TotalWholeSalePrice { get; set; }
        public decimal TotalVatPrice { get; set; }
        public decimal TotalOnSalePrice { get; set; }
        public decimal TotalFees { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalWeight { get; set; }
        public int ReturnablesCount { get; set; }
        public int LinesCount { get; set; }
        public int ProductsCount { get; set; }
        public decimal Donate { get; set; }
        public decimal FeesPrice { get; set; }
        public decimal InternalFeesPrice { get; set; }
        public decimal FeesFixedAmount { get; set; }
        public decimal FeesPercent { get; set; }
        public bool SkipBackgroundProcessing { get; set; }
        public UserProfileViewModel User { get; set; }
        public IEnumerable<PurchaseOrderShortViewModel> PurchaseOrders { get; set; }
    }
}
