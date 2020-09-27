using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class CheckDonationCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-check-donation";

        [JsonConstructor]
        public CheckDonationCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid DonationId { get; set; }
    }
}
