using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Exceptions;

namespace Sheaft.Domain
{
    public class ProfileInformation
    {
        private List<ProfilePicture> _pictures;

        public ProfileInformation()
        {
            _pictures = new List<ProfilePicture>();
        }

        public string Summary { get; private set; }
        public string Description { get; private set; }
        public string Website { get; private set; }
        public string Facebook { get; private set; }
        public string Twitter { get; private set; }
        public string Instagram { get; private set; }
        public virtual ProfilePicture Banner { get; private set; }
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

        public void SetBanner(ProfilePicture picture)
        {
            Banner = picture;
        }
        
        public ProfilePicture AddPicture(ProfilePicture picture)
        {
            _pictures ??= new List<ProfilePicture>();
            _pictures.Add(picture);

            if (_pictures.Count == 1)
                picture.IsDefault = true;

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

            if (existingPicture.IsDefault && _pictures.Any())
                _pictures.First().IsDefault = true;
        }

        public void SetDefaultPicture(Guid id)
        {
            var existingPicture = _pictures.FirstOrDefault(p => p.Id == id);
            if (existingPicture == null)
                throw SheaftException.NotFound();

            existingPicture.IsDefault = true;
            
            var pictures = _pictures.Where(p => p.Id != id);
            foreach (var picture in pictures)
                picture.IsDefault = false;
        }
    }
}