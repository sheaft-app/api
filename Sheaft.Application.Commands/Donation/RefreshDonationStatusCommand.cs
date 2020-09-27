﻿using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class RefreshDonationStatusCommand : Command<TransactionStatus>
    {
        public const string QUEUE_NAME = "command-refresh-donation-status";

        [JsonConstructor]
        public RefreshDonationStatusCommand(RequestUser requestUser, string identifier)
            : base(requestUser)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
}
