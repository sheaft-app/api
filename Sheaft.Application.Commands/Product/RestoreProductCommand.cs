using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RestoreProductCommand : Command<bool>
    {
        public RestoreProductCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
