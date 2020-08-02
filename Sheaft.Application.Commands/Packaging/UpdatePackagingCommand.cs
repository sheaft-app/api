using System;

namespace Sheaft.Application.Commands
{
    public class UpdatePackagingCommand : Command<bool>
    {
        public UpdatePackagingCommand(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal Vat { get; set; }
    }
}
