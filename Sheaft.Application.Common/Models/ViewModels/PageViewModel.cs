using System;

namespace Sheaft.Application.Common.Models.ViewModels
{
    public class PageViewModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UploadedOn { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
    }
}
