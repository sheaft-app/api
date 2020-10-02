using Sheaft.Core;
using Newtonsoft.Json;
using System;

namespace Sheaft.Application.Commands
{
    public class CreateDonationCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateDonationCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid OrderId { get; set; }
    }
}
