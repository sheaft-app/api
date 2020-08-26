using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RegisterOwnerCommand : UserCommand<Guid>
    {
        [JsonConstructor]
        public RegisterOwnerCommand(RequestUser requestUser) : base(requestUser)
        {
            Id = requestUser.Id;
        }

        public string SponsoringCode { get; set; }
        public Guid CompanyId { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
