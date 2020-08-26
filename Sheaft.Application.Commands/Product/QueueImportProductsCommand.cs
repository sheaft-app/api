using System;
using System.IO;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class QueueImportProductsCommand : Command<Guid>
    {
        [JsonConstructor]
        public QueueImportProductsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid CompanyId { get; set; }
        public string FileName { get; set; }
        public Stream FileStream { get; set; }
    }
}
