using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Models.Inputs;

namespace Sheaft.Application.Commands
{
    public class CreateBankAccountCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateBankAccountCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string BIC { get; set; }
        public string IBAN { get; set; }
        public AddressInput Address { get; set; }
    }
}
