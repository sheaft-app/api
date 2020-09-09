using System;
using System.Collections.Generic;
using Sheaft.Interop.Enums;
using Sheaft.Models.Inputs;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class UpdateStoreCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateStoreCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public ProfileKind? Kind { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string Siret { get; set; }
        public string Picture { get; set; }
        public string VatIdentifier { get; set; }
        public bool OpenForNewBusiness { get; set; }
        public AddressInput Address { get; set; }
        public AddressInput BillingAddress { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
    }
}
