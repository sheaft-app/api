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

        public Guid Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileStream { get; set; }
    }
}
