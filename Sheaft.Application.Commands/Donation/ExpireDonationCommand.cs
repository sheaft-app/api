using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class ExpireDonationCommand : Command<bool>
    {
        [JsonConstructor]
        public ExpireDonationCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }

        public Guid DonationId { get; set; }
    }
}
