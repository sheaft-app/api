using System;
using Newtonsoft.Json;
using Sheaft.Core;
using Sheaft.Models.Inputs;

namespace Sheaft.Application.Commands
{
    public class CreateConsumerCommand : ConsumerCommand<Guid>
    {
        [JsonConstructor]
        public CreateConsumerCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string SponsoringCode { get; set; }
        public bool Anonymous { get; set; }
        public AddressInput Address { get; set; }
        public Guid? DepartmentId { get; set; }
    }
}
