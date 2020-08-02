using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class DeleteCompaniesCommand : Command<bool>
    {
        public DeleteCompaniesCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> Ids { get; set; }
        public string Reason { get; set; }
    }
}
