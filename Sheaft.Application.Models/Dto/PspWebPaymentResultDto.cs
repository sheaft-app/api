﻿using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Models
{
    public class PspWebPaymentResultDto : PspPaymentResultDto
    {
        public string RedirectUrl { get; set; }
    }
}