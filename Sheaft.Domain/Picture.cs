using System;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public abstract class Picture: IIdEntity, ITrackCreation, ITrackUpdate
    {
        public Picture(Guid id, string url)
        {
            Id = Id;
            Url = url;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public bool IsDefault { get; internal set; }
        public string Url { get; private set; }
    }

    public class ProfilePicture : Picture
    {
        public ProfilePicture(Guid id, string url) : base(id, url)
        {
        }
    }

    public class ProductPicture : Picture
    {
        public ProductPicture(Guid id, string url) : base(id, url)
        {
        }
    }
}