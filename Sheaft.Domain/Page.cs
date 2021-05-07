using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Page : IIdEntity, ITrackCreation, ITrackUpdate
    {
        public Page(Guid id, string filename, string extension, long size)
        {
            Id = id;
            Filename = filename;
            Extension = extension;
            Size = size;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? UploadedOn { get; private set; }
        public string Filename { get; private set; }
        public string Extension { get; private set; }
        public long Size { get; private set; }
        public Guid DocumentId { get; private set; }
        public byte[] RowVersion { get; private set; }

        public void SetUploaded()
        {
            UploadedOn = DateTimeOffset.UtcNow;
        }
    }
}