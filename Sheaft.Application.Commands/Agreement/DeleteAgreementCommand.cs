using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class DeleteAgreementCommand : Command<bool>
    {
        public DeleteAgreementCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
