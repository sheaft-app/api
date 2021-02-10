using System;

namespace Sheaft.Application.Models.Mailer
{
    public class RgpdExportMailerModel
    {
        public string UserName { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string Name { get; set; }
        public string DownloadUrl { get; set; }
        public string PortalUrl { get; set; }
    }
}
