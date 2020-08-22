using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class DeleteCompanyCommand : Command<bool>
    {
        public DeleteCompanyCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
}
