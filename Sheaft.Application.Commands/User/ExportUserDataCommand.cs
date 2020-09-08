using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Commands
{
    public class ExportUserDataCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-export-user-data";

        [JsonConstructor]
        public ExportUserDataCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
