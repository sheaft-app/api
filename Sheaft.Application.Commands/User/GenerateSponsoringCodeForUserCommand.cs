using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class GenerateUserSponsoringCodeCommand : Command<string>
    {
        public GenerateUserSponsoringCodeCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
