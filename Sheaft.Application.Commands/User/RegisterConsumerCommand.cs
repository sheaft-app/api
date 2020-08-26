using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class RegisterConsumerCommand : UserCommand<Guid>
    {
        [JsonConstructor]
        public RegisterConsumerCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public string SponsoringCode { get; set; }
        public bool Anonymous { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
