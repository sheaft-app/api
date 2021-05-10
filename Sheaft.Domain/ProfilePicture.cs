using System;

namespace Sheaft.Domain
{
    public class ProfilePicture : Picture
    {
        protected ProfilePicture()
        {
        }
        
        public ProfilePicture(Guid id, string url) : base(id, url)
        {
        }

        public Guid UserId { get; private set; }
    }
}