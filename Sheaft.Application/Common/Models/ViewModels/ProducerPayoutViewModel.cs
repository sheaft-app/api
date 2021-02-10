using Sheaft.Domain.Enums;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Sheaft.Application.Models
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
