﻿using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.ViewModels
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
