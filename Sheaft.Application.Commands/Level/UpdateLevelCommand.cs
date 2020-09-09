using Sheaft.Interop.Enums;
using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class UpdateLevelCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateLevelCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int RequiredPoints { get; set; }
    }
}
