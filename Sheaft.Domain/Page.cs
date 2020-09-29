using System;

namespace Sheaft.Domain.Models
{
    public class Page
    {
        public Page(Guid id, string filename, string extension, long size)
        {
            Id = id;
            Filename = filename;
            Extension = extension;
            Size = size;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? UploadedOn { get; set; }
        public string Filename { get; private set; }
        public string Extension { get; private set; }
        public long Size { get; private set; }

        public void SetUploaded()
        {
            UploadedOn = DateTimeOffset.UtcNow;
        }
    }
}