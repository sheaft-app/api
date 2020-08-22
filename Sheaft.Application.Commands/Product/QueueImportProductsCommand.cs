using System;
using System.IO;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class QueueImportProductsCommand : Command<Guid>
    {
        public QueueImportProductsCommand(RequestUser user) : base(user)
        {
        }

        public Guid CompanyId { get; set; }
        public string FileName { get; set; }
        public Stream FileStream { get; set; }
    }
}
