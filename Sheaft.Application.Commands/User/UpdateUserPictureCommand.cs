using System;

namespace Sheaft.Application.Commands
{
    public class UpdateUserPictureCommand : Command<bool>
    {
        public UpdateUserPictureCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Picture { get; set; }
    }
}
