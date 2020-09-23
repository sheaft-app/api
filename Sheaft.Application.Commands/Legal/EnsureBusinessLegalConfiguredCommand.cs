using System;
using Sheaft.Domain.Enums;
using Sheaft.Application.Models;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class EnsureBusinessLegalConfiguredCommand : Command<bool>
    {
        [JsonConstructor]
        public EnsureBusinessLegalConfiguredCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
    }
}
