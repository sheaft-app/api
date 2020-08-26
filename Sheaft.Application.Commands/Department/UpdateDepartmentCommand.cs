using Sheaft.Interop.Enums;
using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class UpdateDepartmentCommand : Command<bool>
    {
        [JsonConstructor]
        public UpdateDepartmentCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
