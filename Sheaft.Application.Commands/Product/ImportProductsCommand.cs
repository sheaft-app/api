using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class ImportProductsCommand : Command<bool>
    {
        [JsonConstructor]
        public ImportProductsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public bool NotifyOnUpdates { get; set; } = true;
    }
}
