using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class ResetAgreementStatusToCommand : Command<bool>
    {
        public ResetAgreementStatusToCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public AgreementStatusKind Status { get; set; }
    }
}
