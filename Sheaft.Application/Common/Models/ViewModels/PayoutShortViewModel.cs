using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class PayoutShortViewModel
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; }
        public TransactionKind Kind { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
    }
}
