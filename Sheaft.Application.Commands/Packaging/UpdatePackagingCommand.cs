using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class UpdatePackagingCommand : Command<bool>
    {
        public UpdatePackagingCommand(RequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal Vat { get; set; }
    }
}
