using System;
using Sheaft.Core;
using Newtonsoft.Json;
using System.IO;

namespace Sheaft.Application.Commands
{
    public class UploadPageCommand : Command<bool>
    {
        [JsonConstructor]
        public UploadPageCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid DocumentId { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public Stream Data { get; set; }
    }
}
