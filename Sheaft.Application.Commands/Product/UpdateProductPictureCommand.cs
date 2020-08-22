using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateProductPictureCommand : Command<bool>
    {
        public UpdateProductPictureCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Picture { get; set; }
    }
}
