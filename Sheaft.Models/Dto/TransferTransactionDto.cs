﻿using System;

namespace Sheaft.Models.Dto
{
    public class TransferTransactionDto : TransactionDto
    {
        public UserProfileDto DebitedUser { get; set; }
        public UserProfileDto CreditedUser { get; set; }
        public PurchaseOrderDto PurchaseOrder { get; set; }
    }
}