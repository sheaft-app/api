using System;
using System.Collections.Generic;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class QueueExportPickingOrderCommand : Command<Guid>
    {
        public QueueExportPickingOrderCommand(RequestUser user) : base(user)
        {
        }

        public IEnumerable<Guid> PurchaseOrderIds { get; set; }
        public string Name { get; set; }
    }
}
