using System;

namespace Sheaft.Mailing
{
    public class BatchMailerModel
    {
        public string Number { get; set; }
        public DateTimeOffset? DLC { get; set; }
        public DateTimeOffset? DLUO { get; set; }
    }
}