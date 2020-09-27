using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class CreateDonationCommand : Command<Guid>
    {
        public const string QUEUE_NAME = "command-create-donation";

        [JsonConstructor]
        public CreateDonationCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }
}
