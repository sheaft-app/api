using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateUserPictureCommand : Command<bool>
    {
        public UpdateUserPictureCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Picture { get; set; }
    }
}
