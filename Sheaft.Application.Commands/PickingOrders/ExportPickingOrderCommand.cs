using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class ExportPickingOrderCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-pickingorders-export";

        public ExportPickingOrderCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid JobId { get; set; }
        public IEnumerable<Guid> PurchaseOrderIds { get; set; }
    }
}
