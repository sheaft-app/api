using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class CheckNewDonationsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckNewDonationsCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }
    }
}
