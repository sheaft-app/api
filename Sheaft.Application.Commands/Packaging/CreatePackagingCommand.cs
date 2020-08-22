using System;
using Sheaft.Core;

namespace Sheaft.Application.Commands
{
    public class CreatePackagingCommand : Command<Guid>
    {
        public CreatePackagingCommand(RequestUser user) : base(user)
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal Vat { get; set; }
    }
}
