using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateTagIconCommand : Command<string>
    {
        [JsonConstructor]
        public UpdateTagIconCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid TagId { get; set; }
        public string Icon { get; set; }
    }
}
