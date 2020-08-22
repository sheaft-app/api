using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RemoveCompanyDataCommand : Command<string>
    {
        public const string QUEUE_NAME = "command-companies-removedata";

        public RemoveCompanyDataCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}
