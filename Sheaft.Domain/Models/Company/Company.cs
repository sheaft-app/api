using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.BaseClass;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events;
using Sheaft.Domain.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Company: IEntity, IHasDomainEvent
    {
        protected Company()
        {
        }

        public Company(string name, string email, string phone, ShippingAddress address)
        {
            if (address == null)
                throw new ValidationException("L'adresse de livraison est requise.");
            
            Name = name;
            Email = email;
            Phone = phone;
            ShippingAddress = address;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public bool Removed { get; private set; }
        public bool OpenForNewContracts { get; set; }
        public CompanyLegals Legals { get; private set; }
        public CompanyDetails Details { get; private set; }
        public CompanyBilling Billing { get; private set; }
        public ShippingAddress ShippingAddress { get; private set; }
        public ICollection<CompanySetting> Settings { get; private set; }
        public ICollection<User> Users { get; private set; }
        public List<DomainEvent> DomainEvents { get; private set; } = new List<DomainEvent>();
        
        public void Restore()
        {
            Removed = false;
        }

        public void SetLegalsInfo(LegalKind kind, string name, string siret, string vatNumber, LegalsAddress address, string registrationCity = null, string registrationCode = null, RegistrationKind? registrationKind = null)
        {
            Legals = new CompanyLegals(Id, kind, name, siret, vatNumber, string.IsNullOrWhiteSpace(vatNumber), address);;
            if(registrationKind.HasValue)
                Legals.SetRegistrationKind(registrationKind.Value, registrationCity, registrationCode);
        }

        public void SetDetailsInfo(string summary, string description, string website, string facebook, string instagram, string twitter, IEnumerable<Picture> pictures)
        {
            Details = new CompanyDetails(Id, summary, description, website, facebook, instagram, twitter);
            Details.SetPictures(pictures);
        }

        public void SetBillingInfo(string name, string iban, string bic, string streetAddress, string postalCode, string city, string country, string line2 = null)
        {
            Billing = new CompanyBilling(Id, iban, bic, new BillingAddress(name,streetAddress, line2, postalCode, city, CountryIsoCode.FR));
        }

        public void AddUser()
        {
            
        }
        
        public CompanySetting GetSetting(SettingKind kind)
        {
            return Settings?.SingleOrDefault(s => s.Setting.Kind == kind);
        }

        public CompanySetting GetSetting(Guid id)
        {
            return Settings?.SingleOrDefault(s => s.SettingId == id);
        }

        public void AddSetting(Setting setting, string value)
        {
            if (Settings == null)
                Settings = new List<CompanySetting>();

            if (Settings.Any(s => s.Setting.Kind == setting.Kind))
                throw new DuplicateResourceException("Le paramètre existe déjà.");
            
            Settings.Add(new CompanySetting(Id, setting, value));
        }

        public void EditSetting(Guid settingId, string value)
        {
            if (Settings == null)
                throw new NotFoundException("Aucun paramètre trouvé.");

            var setting = Settings.SingleOrDefault(s => s.SettingId == settingId);
            if(setting == null)
                throw new NotFoundException("Le paramètre est introuvable.");

            setting.Value = value;
        }

        public void RemoveSetting(Guid settingId)
        {
            if (Settings == null)
                throw new NotFoundException("Aucun paramètre trouvé.");

            var setting = Settings.SingleOrDefault(s => s.SettingId == settingId);
            if(setting == null)
                throw new NotFoundException("Le paramètre est introuvable.");

            Settings.Remove(setting);
        }
    }
}
