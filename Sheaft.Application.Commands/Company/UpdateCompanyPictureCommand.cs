using System;

namespace Sheaft.Application.Commands
{
    public class UpdateCompanyPictureCommand : Command<bool>
    {
        public UpdateCompanyPictureCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Picture { get; set; }
    }
}
