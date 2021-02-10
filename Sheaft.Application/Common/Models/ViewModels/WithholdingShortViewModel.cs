using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{
    public class WithholdingShortViewModel
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; }
        public decimal Credited { get; set; }
        public decimal Debited { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
