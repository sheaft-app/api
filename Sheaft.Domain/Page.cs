using System;

namespace Sheaft.Domain.Models
{
    public class Page
    {
        public Page(Guid id, string filename, string extension, decimal size)
        {
            Id = id;
            Filename = filename;
            Extension = extension;
            Size = size;
        }

        public Guid Id { get; private set; }
        public string Filename { get; private set; }
        public string Extension { get; private set; }
        public decimal Size { get; private set; }
        public bool Uploaded { get; private set; }

        public void SetUploaded()
        {
            Uploaded = true;
        }
    }
}