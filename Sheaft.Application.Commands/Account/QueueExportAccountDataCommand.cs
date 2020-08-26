using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Commands
{
    public class QueueExportAccountDataCommand : Command<Guid>
    {
        [JsonConstructor]
        public QueueExportAccountDataCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
