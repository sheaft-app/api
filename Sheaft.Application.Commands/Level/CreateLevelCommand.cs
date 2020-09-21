using Sheaft.Domain.Enums;
using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class CreateLevelCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateLevelCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string Name { get; set; }
        public int RequiredPoints { get; set; }
    }
}
