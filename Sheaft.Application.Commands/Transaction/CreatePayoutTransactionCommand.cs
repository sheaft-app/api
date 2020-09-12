using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreatePayoutTransactionCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreatePayoutTransactionCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid FromUserId { get; set; }
        public Guid BankAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
