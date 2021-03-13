using System;

namespace Sheaft.Application.Common.Models.Mailer
{
    public class TransactionsExportMailerModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string JobUrl { get; set; }
        public string DownloadUrl { get; set; }
    }
}