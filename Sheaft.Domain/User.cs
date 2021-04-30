using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Core.Extensions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public abstract class User : IEntity
    {
        private List<UserPoint> _points;
        private List<UserSetting> _settings;
        private List<ProfilePicture> _pictures;

        protected User()
        {
        }

        protected User(Guid id, ProfileKind kind, string name, string firstname, string lastname, string email, string phone = null)
        {
            Id = id;

            SetProfileKind(kind);
            SetUserName(name);
            SetEmail(email);
            SetPhone(phone);
            SetFirstname(firstname);
            SetLastname(lastname);

            _points = new List<UserPoint>();
            _settings = new List<UserSetting>();
            
            RefreshPoints();
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Identifier { get; private set; }
        public ProfileKind Kind { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Picture { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string SponsorshipCode { get; private set; }
        public int TotalPoints { get; private set; }
        public string Summary { get; private set; }
        public string Description { get; private set; }
        public string Website { get; private set; }
        public string Facebook { get; private set; }
        public string Twitter { get; private set; }
        public string Instagram { get; private set; }
        public UserAddress Address { get; private set; }
        public virtual Legal Legal { get; protected set; }
        public virtual IReadOnlyCollection<UserPoint> Points => _points.AsReadOnly();
        public virtual IReadOnlyCollection<UserSetting> Settings => _settings.AsReadOnly();
        public virtual IReadOnlyCollection<ProfilePicture> Pictures => _pictures?.AsReadOnly();

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
        
        public void SetFirstname(string firstname)
        {
            if (string.IsNullOrWhiteSpace(firstname))
                throw new ValidationException(MessageKind.User_Firstname_Required);

            FirstName = firstname;

            if (Kind == ProfileKind.Consumer)
                SetUserName($"{FirstName} {LastName}");
        }

        public void SetLastname(string lastname)
        {
            if (string.IsNullOrWhiteSpace(lastname))
                throw new ValidationException(MessageKind.User_Lastname_Required);

            LastName = lastname;

            if (Kind == ProfileKind.Consumer)
                SetUserName($"{FirstName} {LastName}");
        }

        public void SetAddress(UserAddress address)
        {
            Address = address;
        }

        public void SetAddress(string line1, string line2, string zipcode, string city, CountryIsoCode country, Department department, double? longitude = null, double? latitude = null)
        {
            Address = new UserAddress(line1, line2, zipcode, city, country, department, longitude, latitude);
        }

        protected void SetUserName(string name)
        {
            if (name.IsNotNullAndIsEmptyOrWhiteSpace())
                throw new ValidationException(MessageKind.User_Name_Required);

            Name = name;
        }

        public void SetProfileKind(ProfileKind? kind)
        {
            if (!kind.HasValue)
                return;

            Kind = kind.Value;
        }

        public void SetEmail(string email)
        {
            if (email.IsNotNullAndIsEmptyOrWhiteSpace())
                throw new ValidationException(MessageKind.User_Email_Required);

            Email = email;
        }
        public void SetPhone(string phone)
        {
            if (phone == null)
                return;

            Phone = phone;
        }

        public void SetPicture(string picture)
        {
            if (picture == null)
                return;

            Picture = picture;
        }

        public void SetSponsoringCode(string code)
        {
            SponsorshipCode = code;
        }

        public void SetIdentifier(string identifier)
        {
            if (identifier == null)
                return;

            Identifier = identifier;
        }

        public virtual void Close()
        {
            if(Kind != ProfileKind.Consumer)
                return;

            FirstName = string.Empty;
            LastName = string.Empty;
            Name = string.Empty;
            Email = $"{Guid.NewGuid():N}@ano.nym";
            Phone = string.Empty;
            SetAddress("Anonymous", null, Address.Zipcode, "Anonymous", Address.Country, Address.Department);
        }

        public UserPoint AddPoints(PointKind kind, int quantity, DateTimeOffset? createdOn = null)
        {
            if (Points == null)
                _points = new List<UserPoint>();

            var points = new UserPoint(Guid.NewGuid(), kind, quantity, createdOn ?? DateTimeOffset.UtcNow);
            _points.Add(points);
            RefreshPoints();

            return points;
        }

        private void RefreshPoints()
        {
            TotalPoints = _points.Sum(c => c.Quantity);
        }

        public void Restore()
        {
            RemovedOn = null;
        }

        public UserSetting GetSetting(SettingKind kind)
        {
            return Settings?.SingleOrDefault(s => s.Setting.Kind == kind);
        }

        public UserSetting GetSetting(Guid id)
        {
            return Settings?.SingleOrDefault(s => s.SettingId == id);
        }

        public void AddSetting(Setting setting, string value)
        {
            if (Settings == null)
                _settings = new List<UserSetting>();

            if (_settings.Any(s => s.Setting.Kind == setting.Kind))
                throw SheaftException.AlreadyExists();
            
            _settings.Add(new UserSetting(setting, value));
        }

        public void EditSetting(Guid settingId, string value)
        {
            if (Settings == null)
                throw SheaftException.NotFound();

            var setting = _settings.SingleOrDefault(s => s.SettingId == settingId);
            if(setting == null)
                throw SheaftException.NotFound();

            setting.SetValue(value);
        }

        public void RemoveSetting(Guid settingId)
        {
            if (Settings == null)
                throw SheaftException.NotFound();

            var setting = _settings.SingleOrDefault(s => s.SettingId == settingId);
            if(setting == null)
                throw SheaftException.NotFound();

            _settings.Remove(setting);
        }
    }

    public class Admin : User
    {
        protected Admin()
        {
        }

        public Admin(Guid id, string name, string firstname, string lastname, string email)
            : base(id, ProfileKind.Admin, name, firstname, lastname, email) { }
    }

    public class Support : User
    {
        protected Support()
        {
        }
        public Support(Guid id, string name, string firstname, string lastname, string email)
            : base(id, ProfileKind.Support, name, firstname, lastname, email) { }
    }
}