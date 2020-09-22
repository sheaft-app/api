﻿using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Commands
{
    public class CreatePayoutForTransfersCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-create-payout-for-transfers";

        [JsonConstructor]
        public CreatePayoutForTransfersCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
