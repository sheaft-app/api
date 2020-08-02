using System;
using Sheaft.Interop;

namespace Sheaft.Application.Commands
{
    public class GenerateUserSponsoringCodeCommand : Command<string>
    {
        public GenerateUserSponsoringCodeCommand(IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
