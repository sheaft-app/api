using System;

namespace Sheaft.Mailing
{
    public class ProductImportMailerModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string JobUrl { get; set; }
        public string PortalUrl { get; set; }
    }
}
