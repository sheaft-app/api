using Sheaft.Interop.Enums;
using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreateUserPointsCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-users-points-create";

        public CreateUserPointsCommand(RequestUser user) : base(user)
        {
        }

        public PointKind Kind { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public Guid UserId { get; set; }
    }
}
