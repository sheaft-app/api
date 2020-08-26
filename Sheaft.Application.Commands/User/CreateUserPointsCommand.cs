using Sheaft.Interop.Enums;
using System;
using Sheaft.Core;
using Newtonsoft.Json;

namespace Sheaft.Application.Commands
{
    public class CreateUserPointsCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-users-points-create";

        [JsonConstructor]
        public CreateUserPointsCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public PointKind Kind { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public Guid UserId { get; set; }
    }
}
