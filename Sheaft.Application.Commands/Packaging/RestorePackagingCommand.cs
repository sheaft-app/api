using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RestorePackagingCommand : Command<bool>
    {
        public RestorePackagingCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
