﻿using System;

namespace Sheaft.Models.Dto
{
    public class PayoutTransactionDto : TransactionDto
    {
        public UserProfileDto DebitedUser { get; set; }
    }
}