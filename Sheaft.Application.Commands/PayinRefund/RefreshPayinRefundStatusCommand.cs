﻿using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class RefreshPayinRefundStatusCommand : Command<TransactionStatus>
    {
        public const string QUEUE_NAME = "command-refresh-payin-refund-status";

        [JsonConstructor]
        public RefreshPayinRefundStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
}
