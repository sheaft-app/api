using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class ImportProductsCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-products-import";

        [JsonConstructor]
        public ImportProductsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Uri { get; set; }
    }
}
