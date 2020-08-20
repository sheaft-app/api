using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class DeleteAgreementCommand : Command<bool>
    {
        public DeleteAgreementCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
