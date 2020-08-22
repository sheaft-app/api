using Sheaft.Core;
using System;

namespace Sheaft.Application.Commands
{
    public class QueueExportAccountDataCommand : Command<Guid>
    {
        public QueueExportAccountDataCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
