using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class DeleteProductCommand : Command<bool>
    {
        public DeleteProductCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
