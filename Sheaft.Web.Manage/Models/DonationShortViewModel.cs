using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Web.Manage.Models
{
    public class DonationShortViewModel
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
