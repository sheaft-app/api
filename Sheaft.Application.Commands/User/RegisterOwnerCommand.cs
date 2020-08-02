using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class RegisterOwnerCommand : UserCommand<Guid>
    {
        public RegisterOwnerCommand(Interop.IRequestUser user) : base(user)
        {
            Id = user.Id;
        }

        public string SponsoringCode { get; set; }
        public Guid CompanyId { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
