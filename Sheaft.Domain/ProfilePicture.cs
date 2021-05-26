using System;

namespace Sheaft.Domain
{
    public class ProfilePicture : Picture
    {
        protected ProfilePicture()
        {
        }
        
        public ProfilePicture(Guid id, string url, int position) : base(id, url, position)
        {
        }

        public Guid UserId { get; private set; }
    }
}