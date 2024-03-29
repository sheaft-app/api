﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Core.Extensions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public abstract class User : IEntity
    {
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

            Points = new List<UserPoint>();
            Settings = new List<UserSetting>();
            Pictures = new List<ProfilePicture>();
            
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
        public int PointsCount { get; private set; }
        public int SettingsCount { get; private set; }
        public int PicturesCount { get; private set; }
        public virtual Legal Legal { get; protected set; }
        public virtual ICollection<UserPoint> Points  { get; private set; }
        public virtual ICollection<UserSetting> Settings { get; private set; }
        public virtual ICollection<ProfilePicture> Pictures { get; private set; }
        public byte[] RowVersion { get; private set; }

        public void ClearPictures()
        {
            if (Pictures == null || Pictures.Any())
                Pictures = new List<ProfilePicture>();
        }
        
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
        
        public void AddPicture(ProfilePicture picture)
        {
            Pictures ??= new List<ProfilePicture>();
            Pictures.Add(picture);
            PicturesCount = Pictures?.Count ?? 0;
        }
        
        public void RemovePicture(Guid id)
        {
            if (Pictures == null || Pictures.Any())
                throw SheaftException.NotFound("Cet utilisateur ne contient aucune images.");

            var existingPicture = Pictures.FirstOrDefault(p => p.Id == id);
            if (existingPicture == null)
                throw SheaftException.NotFound("L'image est introuvable.");
            
            Pictures.Remove(existingPicture);
            PicturesCount = Pictures?.Count ?? 0;
        }
        
        public void SetFirstname(string firstname)
        {
            if (string.IsNullOrWhiteSpace(firstname))
                throw SheaftException.Validation("Le prénom est requis.");

            FirstName = firstname;

            if (Kind == ProfileKind.Consumer)
                SetUserName($"{FirstName} {LastName}");
        }

        public void SetLastname(string lastname)
        {
            if (string.IsNullOrWhiteSpace(lastname))
                throw SheaftException.Validation("Le nom est requis.");

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
                throw SheaftException.Validation("Le nom d'utilisateur est requis.");

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
                throw SheaftException.Validation("L'email est requis.");

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

        public void AddPoints(PointKind kind, int quantity, DateTimeOffset? createdOn = null)
        {
            if (Points == null)
                Points = new List<UserPoint>();

            var points = new UserPoint(this, Guid.NewGuid(), kind, quantity, createdOn ?? DateTimeOffset.UtcNow);
            Points.Add(points);
            RefreshPoints();
        }

        private void RefreshPoints()
        {
            TotalPoints = Points.Sum(c => c.Quantity);
            PointsCount = Points?.Count ?? 0;
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
                Settings = new List<UserSetting>();

            if (Settings.Any(s => s.Setting.Kind == setting.Kind))
                throw SheaftException.AlreadyExists("Le paramètre existe déjà.");
            
            Settings.Add(new UserSetting(setting, value));
            SettingsCount = Settings?.Count ?? 0;
        }

        public void EditSetting(Guid settingId, string value)
        {
            if (Settings == null)
                throw SheaftException.NotFound("Aucun paramètre trouvé.");

            var setting = Settings.SingleOrDefault(s => s.SettingId == settingId);
            if(setting == null)
                throw SheaftException.NotFound("Le paramètre est introuvable.");

            setting.SetValue(value);
        }

        public void RemoveSetting(Guid settingId)
        {
            if (Settings == null)
                throw SheaftException.NotFound("Aucun paramètre trouvé.");

            var setting = Settings.SingleOrDefault(s => s.SettingId == settingId);
            if(setting == null)
                throw SheaftException.NotFound("Le paramètre est introuvable.");

            Settings.Remove(setting);
            SettingsCount = Settings?.Count ?? 0;
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