﻿using System;

namespace Sheaft.Models.Dto
{
    public class TransactionDto : BaseTransactionDto
    {
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        public decimal Fees { get; set; }
        public decimal Debited { get; set; }
        public decimal Credited { get; set; }
        public UserProfileDto DebitedUser { get; set; }
        public UserProfileDto CreditedUser { get; set; }
    }
}