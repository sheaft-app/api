using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdateCompanyPictureCommand : Command<bool>
    {
        public UpdateCompanyPictureCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Picture { get; set; }
    }
}
