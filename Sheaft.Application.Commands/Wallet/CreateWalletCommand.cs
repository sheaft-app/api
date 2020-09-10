using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Interop.Enums;

namespace Sheaft.Application.Commands
{
    public class CreateWalletCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateWalletCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string Name { get; set; }
        public WalletKind Kind { get; set; }
    }
}
