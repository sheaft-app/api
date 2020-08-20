using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{

    public class ResetAgreementCommand : Command<bool>
    {
        public ResetAgreementCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
