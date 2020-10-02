using Sheaft.Domain.Enums;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{

    public class ProducerPayoutsViewModel
    {
        public IEnumerable<ProducerTransferViewModel> Transfers { get; set; }
        public Guid Id { get; set; }
        public decimal Total { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
