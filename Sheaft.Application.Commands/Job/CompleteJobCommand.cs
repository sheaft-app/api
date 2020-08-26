using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CompleteJobCommand : Command<bool>
    {
        [JsonConstructor]
        public CompleteJobCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string FileUrl { get; set; }
    }
}
