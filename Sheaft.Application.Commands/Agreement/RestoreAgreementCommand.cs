using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class RestoreAgreementCommand : Command<bool>
    {
        public RestoreAgreementCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
