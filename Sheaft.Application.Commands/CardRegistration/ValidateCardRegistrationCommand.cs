using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class ValidateCardRegistrationCommand : Command<Guid>
    {
        [JsonConstructor]
        public ValidateCardRegistrationCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid CardId { get; set; }
        public string RegistrationData { get; set; }
        public bool? Remember { get; set; }
    }
}
