using System;

namespace Sheaft.Mailing
{
    public class BatchMailerModel
    {
        public string Number { get; set; }
        public DateTimeOffset? DLC { get; set; }
        public DateTimeOffset? DDM { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string Comment { get; set; }
        public string PortalUrl { get; set; }
        public string Username { get; set; }
    }
}