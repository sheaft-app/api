using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RemoveCompanyDataCommand : Command<string>
    {
        public const string QUEUE_NAME = "command-companies-removedata";

        [JsonConstructor]
        public RemoveCompanyDataCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}
