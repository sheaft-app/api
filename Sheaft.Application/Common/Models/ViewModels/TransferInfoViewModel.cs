using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class TransferInfoViewModel
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; }
        public decimal Credited { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
