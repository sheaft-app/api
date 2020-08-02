using System;

namespace Sheaft.Application.Commands
{
    public class ImportProductsCommand : Command<bool>
    {
        public const string QUEUE_NAME = "importproducts";

        public ImportProductsCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Uri { get; set; }
    }
}
