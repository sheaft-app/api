using System;

namespace Sheaft.Application.Commands
{
    public class ExportAccountDataCommand : Command<bool>
    {
        public const string QUEUE_NAME = "exportaccountdata";

        public ExportAccountDataCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
