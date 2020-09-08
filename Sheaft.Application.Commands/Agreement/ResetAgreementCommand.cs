using Sheaft.Interop.Enums;
using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class ResetAgreementStatusToCommand : Command<bool>
    {
        [JsonConstructor]
        public ResetAgreementStatusToCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
    }
}
