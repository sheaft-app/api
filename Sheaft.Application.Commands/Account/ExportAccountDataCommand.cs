using Newtonsoft.Json;
using Sheaft.Core;
using System;

namespace Sheaft.Application.Commands
{
    public class ExportAccountDataCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-accounts-exportdata";

        [JsonConstructor]
        public ExportAccountDataCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
