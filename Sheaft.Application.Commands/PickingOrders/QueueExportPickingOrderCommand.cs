using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class QueueExportPickingOrderCommand : Command<Guid>
    {
        [JsonConstructor]
        public QueueExportPickingOrderCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public IEnumerable<Guid> PurchaseOrderIds { get; set; }
        public string Name { get; set; }
    }
}
