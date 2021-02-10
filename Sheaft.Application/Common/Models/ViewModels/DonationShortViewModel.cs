using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.ViewModels
{
    public class DonationShortViewModel
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
