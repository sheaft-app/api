using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RegisterConsumerCommand : ConsumerCommand<Guid>
    {
        [JsonConstructor]
        public RegisterConsumerCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string SponsoringCode { get; set; }
    }
}
