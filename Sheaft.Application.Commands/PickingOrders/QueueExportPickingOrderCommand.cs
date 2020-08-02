using System;
using System.Collections.Generic;

namespace Sheaft.Application.Commands
{
    public class QueueExportPickingOrderCommand : Command<Guid>
    {
        public QueueExportPickingOrderCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> PurchaseOrderIds { get; set; }
        public string Name { get; set; }
    }
}
