using System;
using Newtonsoft.Json;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateUserCommand : UserCommand<bool>
    {
        [JsonConstructor]
        public UpdateUserCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid? DepartmentId { get; set; }
        public bool Anonymous { get; set; }
    }
}
