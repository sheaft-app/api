using System;

namespace Sheaft.Application.Commands
{
    public class ExportAccountDataCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-accounts-exportdata";

        public ExportAccountDataCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
