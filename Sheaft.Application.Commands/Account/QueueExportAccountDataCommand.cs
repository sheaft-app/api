using System;

namespace Sheaft.Application.Commands
{
    public class QueueExportAccountDataCommand : Command<Guid>
    {
        public QueueExportAccountDataCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
