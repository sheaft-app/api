using System;
using System.Collections.Generic;
using Sheaft.Domain.Enum;

namespace Sheaft.Mailing
{
    public class OrderSummaryMailerModel
    {
        public string SenderName { get; set; }
        public ProfileKind SenderKind { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal OnSalePrice { get; set; }
        public int ProductsCount { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string OrderId { get; set; }
        public string MyOrdersUrl { get; set; }
        public decimal Donation { get; set; }
        public decimal Fees { get; set; }
        public IEnumerable<ProducerMailerModel> Producers { get; set; }
    }
}