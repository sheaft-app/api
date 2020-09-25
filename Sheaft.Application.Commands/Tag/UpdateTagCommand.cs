using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Domain.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class UpdateTagCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateTagCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TagKind Kind { get; set; }
        public string Picture { get; set; }
    }
}
