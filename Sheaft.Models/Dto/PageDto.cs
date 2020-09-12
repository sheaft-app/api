using System;

namespace Sheaft.Models.Dto
{
    public class PageDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
    }
}