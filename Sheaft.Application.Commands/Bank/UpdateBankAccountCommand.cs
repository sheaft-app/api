using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Application.Models;

namespace Sheaft.Application.Commands
{
    public class UpdateBankAccountCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateBankAccountCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string BIC { get; set; }
        public string IBAN { get; set; }
        public AddressInput Address { get; set; }
    }
}
