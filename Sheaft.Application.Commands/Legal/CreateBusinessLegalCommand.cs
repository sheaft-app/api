using System;
using Sheaft.Domain.Enums;
using Sheaft.Application.Models;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreateBusinessLegalCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateBusinessLegalCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public LegalKind Kind { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string VatIdentifier { get; set; }
        public string Siret { get; set; }
        public OwnerInput Owner { get; set; }
        public AddressInput Address { get; set; }
    }
}
