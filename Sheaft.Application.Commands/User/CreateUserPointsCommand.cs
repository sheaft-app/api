using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class CreateUserPointsCommand : Command<bool>
    {
        public const string QUEUE_NAME = "command-users-points-create";

        public CreateUserPointsCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public PointKind Kind { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public Guid UserId { get; set; }
    }
}
