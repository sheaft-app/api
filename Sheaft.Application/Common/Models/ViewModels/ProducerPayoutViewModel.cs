using System;

namespace Sheaft.Application.Common.Models.ViewModels
{
    public class ProducerPayoutViewModel
    {
        public Guid TransferId { get; set; }
        public Guid ProducerId { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
