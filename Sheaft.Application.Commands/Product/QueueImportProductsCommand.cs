using System;
using System.IO;

namespace Sheaft.Application.Commands
{
    public class QueueImportProductsCommand : Command<Guid>
    {
        public QueueImportProductsCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid CompanyId { get; set; }
        public string FileName { get; set; }
        public Stream FileStream { get; set; }
    }
}
