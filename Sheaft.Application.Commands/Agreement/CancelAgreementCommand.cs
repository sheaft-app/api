using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class CancelAgreementCommand : Command<bool>
    {
        public CancelAgreementCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
}
