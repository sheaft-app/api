using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RestoreAgreementCommand : Command<bool>
    {
        public RestoreAgreementCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
