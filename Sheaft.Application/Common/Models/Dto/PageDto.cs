using System;

namespace Sheaft.Application.Common.Models.Dto
{
    public class PageDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
    }
}