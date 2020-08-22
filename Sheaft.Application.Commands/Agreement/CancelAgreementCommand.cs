using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CancelAgreementCommand : Command<bool>
    {
        public CancelAgreementCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
}
