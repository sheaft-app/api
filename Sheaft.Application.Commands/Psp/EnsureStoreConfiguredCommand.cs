using Sheaft.Core;
using Newtonsoft.Json;
using System;
using Sheaft.Domain.Enums;

namespace Sheaft.Application.Commands
{
    public class EnsureStoreConfiguredCommand : Command<bool>
    {
        [JsonConstructor]
        public EnsureStoreConfiguredCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
