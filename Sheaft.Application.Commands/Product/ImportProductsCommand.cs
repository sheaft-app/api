using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class ImportProductsCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-products-import";

        public ImportProductsCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Uri { get; set; }
    }
}
