using Sheaft.Core;
using Newtonsoft.Json;
using System;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class CheckStoreConfigurationCommand : Command<bool>
    {
        [JsonConstructor]
        public CheckStoreConfigurationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
