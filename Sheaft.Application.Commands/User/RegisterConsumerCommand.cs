using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RegisterConsumerCommand : UserCommand<Guid>
    {
        public RegisterConsumerCommand(RequestUser user) : base(user)
        {
        }

        public string SponsoringCode { get; set; }
        public bool Anonymous { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
