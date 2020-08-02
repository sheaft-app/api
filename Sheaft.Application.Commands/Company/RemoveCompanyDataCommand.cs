using System;

namespace Sheaft.Application.Commands
{
    public class RemoveCompanyDataCommand : Command<string>
    {
        public const string QUEUE_NAME = "removecompanydata";

        public RemoveCompanyDataCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}
