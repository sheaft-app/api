using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RefuseAgreementCommand : Command<bool>
    {
        public RefuseAgreementCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
}
