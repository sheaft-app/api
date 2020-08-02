using System;

namespace Sheaft.Application.Commands
{
    public class DeleteCompanyCommand : Command<bool>
    {
        public DeleteCompanyCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
}
