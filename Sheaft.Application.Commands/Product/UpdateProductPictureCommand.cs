using System;

namespace Sheaft.Application.Commands
{
    public class UpdateProductPictureCommand : Command<bool>
    {
        public UpdateProductPictureCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Picture { get; set; }
    }
}
