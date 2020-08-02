using MediatR;
using System;

namespace Sheaft.Application.Events
{
    public class ProductImportSucceededEvent : Event
    {
        public const string QUEUE_NAME = "eventproductimportsucceeded";
        public const string MAILING_TEMPLATE_ID = "d-e142a041f6de42779ed3e704df0026b6";

        public ProductImportSucceededEvent(Interop.IRequestUser user) : base(user)
        {
        }

        public Guid Id { get; set; }
    }
}
