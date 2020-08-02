using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class RefuseAgreementsCommand : Command<bool>
    {
        public RefuseAgreementsCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public string Reason { get; set; }
    }
}
