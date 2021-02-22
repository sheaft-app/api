using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class ProfileInformation : IIdEntity
    {
        private List<ProfilePicture> _pictures;

        protected ProfileInformation()
        {
        }
        
        public ProfileInformation(User user)
        {
            Id = user.Id;
            _pictures = new List<ProfilePicture>();
        }

        public Guid Id { get; private set; }
        public string Summary { get; private set; }
        public string Description { get; private set; }
        public string Website { get; private set; }
        public string Facebook { get; private set; }
        public string Twitter { get; private set; }
        public string Instagram { get; private set; }
        public virtual IReadOnlyCollection<ProfilePicture> Pictures => _pictures.AsReadOnly();

        public void SetSummary(string summary)
        {
            if (summary == null)
                return;

            Summary = summary;
        }

        public void SetDescription(string description)
        {
            if (description == null)
                return;

            Description = description;
        }

        public void SetWebsite(string website)
        {
            if (website == null)
                return;

            Website = website;
        }

        public void SetFacebook(string facebook)
        {
            if (facebook == null)
                return;

            Facebook = facebook;
        }

        public void SetTwitter(string twitter)
        {
            if (twitter == null)
                return;

            Twitter = twitter;
        }

        public void SetInstagram(string instagram)
        {
            if (instagram == null)
                return;

            Instagram = instagram;
        }
        
        public ProfilePicture AddPicture(ProfilePicture picture)
        {
            _pictures ??= new List<ProfilePicture>();
            _pictures.Add(picture);

            return picture;
        }
        
        public void RemovePicture(Guid id)
        {
            if (_pictures == null || _pictures.Any())
                throw SheaftException.NotFound();

            var existingPicture = _pictures.FirstOrDefault(p => p.Id == id);
            if (existingPicture == null)
                throw SheaftException.NotFound();
            
            _pictures.Remove(existingPicture);
        }
    }
}