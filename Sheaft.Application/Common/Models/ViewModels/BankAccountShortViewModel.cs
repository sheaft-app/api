using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class BankAccountShortViewModel
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; }
        public bool IsActive { get; set; }
        public string IBAN { get; set; }
        public string BIC { get; set; }
        public string Owner { get; set; }
    }
}
