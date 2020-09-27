using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{
    public class DonationShortViewModel
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
