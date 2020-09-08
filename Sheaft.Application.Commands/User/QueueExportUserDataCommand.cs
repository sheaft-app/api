using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Commands
{
    public class QueueExportUserDataCommand : Command<Guid>
    {
        [JsonConstructor]
        public QueueExportUserDataCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
