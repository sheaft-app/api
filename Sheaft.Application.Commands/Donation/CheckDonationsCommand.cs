using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class CheckDonationsCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckDonationsCommand(RequestUser requestUser)
            : base(requestUser)
        {
        }
    }
}
