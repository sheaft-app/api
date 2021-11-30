using System;
using System.Collections.Generic;
using System.Linq;
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

        public Company(string name, string email, ShippingAddress address, bool openForBusiness = true, string phone = null)
        {
            if (address == null)
                throw new ValidationException("L'adresse du siège social est requise.");
            
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Phone = phone;
            OpenForNewContracts = openForBusiness;
            ShippingAddress = address;
        }

        public Guid Id { get; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public bool Removed { get; private set; }
        public bool OpenForNewContracts { get; private set; }
        public CompanyLegals Legals { get; private set; }
        public CompanyDetails Details { get; private set; }
        public CompanyBilling Billing { get; private set; }
        public ShippingAddress ShippingAddress { get; private set; }
        public ICollection<CompanySetting> Settings { get; private set; }
        public ICollection<Catalog> Catalogs { get; private set; }
        public List<DomainEvent> DomainEvents { get; private set; } = new List<DomainEvent>();
        
        public void Restore()
        {
            Removed = false;
        }

        public CompanyLegals SetLegals(LegalKind kind, string name, string siret, string vatIdentifier, LegalsAddress address, string registrationCity = null, string registrationCode = null, RegistrationKind? registrationKind = null)
        {
            var legals = new CompanyLegals(Id, kind, name, siret, vatIdentifier, string.IsNullOrWhiteSpace(vatIdentifier), address);
            if(registrationKind.HasValue)
                legals.SetRegistrationKind(registrationKind.Value, registrationCity, registrationCode);
            
            Legals = legals;
            return legals;
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
