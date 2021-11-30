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
    public class Distribution : IEntity, IHasDomainEvent
    {
        protected Distribution()
        {
        }

        public Distribution(DistributionKind kind, Company supplier, bool enabled, DistributionAddress address,
            IEnumerable<DistributionSchedule> openingHours, string name, string description = null)
        {
            Id = Guid.NewGuid();
            Kind = kind;
            Name = name;
            IsEnabled = enabled;
            Description = description;
            Address = address;
            Supplier = supplier;
            SupplierId = supplier.Id;

            SetDeliveryHours(openingHours);
            
            Closings = new List<DistributionClosing>();
            OpeningHours = new List<DistributionSchedule>();
            DomainEvents = new List<DomainEvent>();
        }

        public Guid Id { get; private set; }
        public DistributionKind Kind { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public bool Removed { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int? LockOrderHoursBeforeDelivery { get; private set; }
        public int? MaxPurchaseOrdersPerTimeSlot { get; private set; }
        public decimal? DeliveryFeesMinPurchaseOrdersAmount { get; private set; }
        public decimal? DeliveryFeesWholeSalePrice { get; private set; }
        public DeliveryFeesApplication? ApplyDeliveryFeesWhen { get; private set; }
        public decimal? AcceptPurchaseOrdersWithAmountGreaterThan { get; private set; }
        public bool IsEnabled { get; private set; }
        public bool IsAutoAcceptingPurchaseOrders { get; private set; }
        public bool IsAutoCompletingPurchaseOrders { get; private set; }
        public DistributionAddress Address { get; private set; }
        public Guid SupplierId { get; private set; }
        public Company Supplier { get; private set; }
        public ICollection<DistributionSchedule> OpeningHours { get; private set; }
        public ICollection<DistributionClosing> Closings { get; private set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        
        public void Restore()
        {
            Removed = false;
        }

        public void SetDeliveryHours(IEnumerable<DistributionSchedule> deliveryHours)
        {
            if (OpeningHours == null)
                OpeningHours = new List<DistributionSchedule>();

            OpeningHours = deliveryHours.ToList();
        }

        public void SetAcceptPurchaseOrdersWithAmountGreaterThan(decimal? amount)
        {
            if (amount is < 0)
                throw new ValidationException("Le montant minimum de commande doit être supérieur ou égal à 0€");

            AcceptPurchaseOrdersWithAmountGreaterThan = amount;
        }

        public void SetDeliveryFees(DeliveryFeesApplication? feesApplication, decimal? fees, decimal? minAmount)
        {
            if (fees is <= 0)
                throw new ValidationException("Le forfait de livraison doit être supérieur à 0€");
            
            if (minAmount is <= 0)
                throw new ValidationException("Le montant minimum de commande pour appliquer le forfait doit être supérieur à 0€");
            
            if(feesApplication is DeliveryFeesApplication.TotalLowerThanPurchaseOrderAmount && !minAmount.HasValue)
                throw new ValidationException("Le montant minimum de commande pour appliquer le forfait est requis.");
            
            if(feesApplication is DeliveryFeesApplication.TotalLowerThanPurchaseOrderAmount && minAmount < AcceptPurchaseOrdersWithAmountGreaterThan)
                throw new ValidationException("Le montant minimum de commande pour appliquer le forfait de livraison doit être supérieur au montant minimum d'acceptation de commande.");

            ApplyDeliveryFeesWhen = feesApplication;
            DeliveryFeesMinPurchaseOrdersAmount = minAmount;
            
            DeliveryFeesWholeSalePrice = fees.HasValue ? Math.Round(fees.Value, 2, MidpointRounding.AwayFromZero) : null;
        }

        public void SetLockOrderHoursBeforeDelivery(int? lockOrderHoursBeforeDelivery)
        {
            LockOrderHoursBeforeDelivery = lockOrderHoursBeforeDelivery;
        }

        public void SetAddress(string line1, string line2, string zipcode, string city, CountryIsoCode country,
            double? longitude = null, double? latitude = null)
        {
            Address = new DistributionAddress(line1, line2, zipcode, city, country, longitude, latitude);
        }

        public void AddClosing(DistributionClosing closing)
        {
            if (Closings == null)
                Closings = new List<DistributionClosing>();

            Closings.Add(closing);
        }

        public void RemoveClosings(IEnumerable<Guid> ids)
        {
            foreach (var id in ids)
                RemoveClosing(id);
        }

        public void RemoveClosing(Guid id)
        {
            var closing = Closings.SingleOrDefault(r => r.Id == id);
            if (closing == null)
                throw new NotFoundException("La plage de fermeture pour ce mode de livraison est introuvable.");

            Closings.Remove(closing);
        }
    }
}