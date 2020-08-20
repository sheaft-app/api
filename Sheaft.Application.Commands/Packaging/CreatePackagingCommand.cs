using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Application.Commands
{
    public class CreatePackagingCommand : Command<Guid>
    {
        public CreatePackagingCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal Vat { get; set; }
    }
}
