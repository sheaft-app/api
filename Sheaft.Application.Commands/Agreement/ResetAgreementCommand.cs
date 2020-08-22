using Sheaft.Interop.Enums;
using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class ResetAgreementStatusToCommand : Command<bool>
    {
        public ResetAgreementStatusToCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public AgreementStatusKind Status { get; set; }
    }
}
