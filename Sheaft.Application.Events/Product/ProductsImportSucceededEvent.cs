using Sheaft.Core;
using System;
using Newtonsoft.Json;

namespace Sheaft.Application.Events
{
    public class ProductImportSucceededEvent : Event
    {
        public const string MAILING_TEMPLATE_ID = "d-e142a041f6de42779ed3e704df0026b6";

        [JsonConstructor]
        public ProductImportSucceededEvent(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid JobId { get; set; }
    }
}
