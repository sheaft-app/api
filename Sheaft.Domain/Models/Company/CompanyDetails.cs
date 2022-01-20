using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.BaseClass;

namespace Sheaft.Domain
{
    public class CompanyDetails
    {
        protected CompanyDetails()
        {
        }
        
        internal CompanyDetails(Guid companyId, string summary, string description = null, string website = null, string facebook = null, string instagram = null, string twitter = null)
        {
            CompanyId = companyId;
            Summary = summary;
            Description = description;
            Website = website;
            Facebook = facebook;
            Instagram = instagram;
            Twitter = twitter;
        }

        public Guid CompanyId { get; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public ICollection<CompanyPicture> Pictures { get; private set; }
        public ICollection<CompanyClosing> Closings { get; private set; }
        public ICollection<CompanyTag> Tags { get; private set; }
        public ICollection<CompanySchedule> OpeningHours { get; private set; }
                
        public void SetOpeningHours(IEnumerable<CompanySchedule> openingHours)
        {
            if (openingHours == null)
                return;

            if (OpeningHours == null || OpeningHours.Any())
                OpeningHours = new List<CompanySchedule>();

            OpeningHours = openingHours.ToList();
        }

        public void SetTags(IEnumerable<Tag> tags)
        {
            if (tags == null)
                return;

            if (Tags == null || Tags.Any())
                Tags = new List<CompanyTag>();

            Tags = tags.Select(t => new CompanyTag(t)).ToList();
        }

        public void SetPictures(IEnumerable<Picture> pictures)
        {
            if (pictures == null)
                return;

            if (Pictures == null || Pictures.Any())
                Pictures = new List<CompanyPicture>();

            Pictures = pictures.Select(t => new CompanyPicture(t.Url, t.Position)).ToList();
        }
    }
}