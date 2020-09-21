using System;
using System.Collections.Generic;
using Sheaft.Application.Models;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{

    public class RegisterStoreCommand : Command<Guid>
    {
        [JsonConstructor]
        public RegisterStoreCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string VatIdentifier { get; set; }
        public string Picture { get; set; }
        public string Siret { get; set; }
        public string SponsoringCode { get; set; }
        public FullAddressInput Address { get; set; }
        public bool OpenForNewBusiness { get; set; }
        public IEnumerable<Guid> Tags { get; set; }
        public IEnumerable<TimeSlotGroupInput> OpeningHours { get; set; }
    }
}
