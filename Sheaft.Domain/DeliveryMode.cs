﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class DeliveryMode : IEntity, IHasDomainEvent
    {
        protected DeliveryMode()
        {
        }

        public DeliveryMode(Guid id, DeliveryKind kind, Producer producer, bool available, DeliveryAddress address,
            IEnumerable<DeliveryHours> openingHours, string name, string description = null)
        {
            Id = id;
            Kind = kind;
            Description = description;

            Address = address;
            Producer = producer;
            ProducerId = producer.Id;

            Closings = new List<DeliveryClosing>();
            DeliveryHours = new List<DeliveryHours>();

            SetName(name);
            SetDeliveryHours(openingHours);
            SetAvailability(available);

            DomainEvents = new List<DomainEvent>();
        }

        public Guid Id { get; private set; }
        public DeliveryKind Kind { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public int? LockOrderHoursBeforeDelivery { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int? MaxPurchaseOrdersPerTimeSlot { get; private set; }
        public decimal? DeliveryFeesMinPurchaseOrdersAmount { get; private set; }
        public decimal? DeliveryFeesWholeSalePrice { get; private set; }
        public decimal? DeliveryFeesVatPrice { get; private set; }
        public decimal? DeliveryFeesOnSalePrice { get; private set; }
        public DeliveryFeesApplication? ApplyDeliveryFeesWhen { get; private set; }
        public decimal? AcceptPurchaseOrdersWithAmountGreaterThan { get; private set; }
        public bool Available { get; private set; }
        public bool AutoAcceptRelatedPurchaseOrder { get; private set; }
        public bool AutoCompleteRelatedPurchaseOrder { get; private set; }
        public DeliveryAddress Address { get; private set; }
        public Guid ProducerId { get; private set; }
        public virtual Producer Producer { get; private set; }
        public int DeliveryHoursCount { get; private set; }
        public int ClosingsCount { get; private set; }
        public virtual ICollection<DeliveryHours> DeliveryHours { get; private set; }
        public virtual ICollection<DeliveryClosing> Closings { get; private set; }
        public virtual ICollection<Agreement> Agreements { get; private set; }

        public void SetDeliveryHours(IEnumerable<DeliveryHours> deliveryHours)
        {
            if (DeliveryHours == null)
                DeliveryHours = new List<DeliveryHours>();

            DeliveryHours = deliveryHours.ToList();
            DeliveryHoursCount = DeliveryHours?.Count ?? 0;
        }

        public void SetAcceptPurchaseOrdersWithAmountGreaterThan(decimal? amount)
        {
            if (amount is < 0)
                throw SheaftException.Validation("Le montant minimum de commande doit être supérieur ou égal à 0€");

            AcceptPurchaseOrdersWithAmountGreaterThan = amount;
        }

        public void SetDeliveryFees(DeliveryFeesApplication? feesApplication, decimal? fees, decimal? minAmount)
        {
            if (fees is <= 0)
                throw SheaftException.Validation("Le forfait de livraison doit être supérieur à 0€");
            
            if (minAmount is <= 0)
                throw SheaftException.Validation("Le montant minimum de commande pour appliquer le forfait doit être supérieur à 0€");
            
            if(feesApplication is DeliveryFeesApplication.TotalLowerThanPurchaseOrderAmount && !minAmount.HasValue)
                throw SheaftException.Validation("Le montant minimum de commande pour appliquer le forfait est requis.");
            
            if(feesApplication is DeliveryFeesApplication.TotalLowerThanPurchaseOrderAmount && minAmount < AcceptPurchaseOrdersWithAmountGreaterThan)
                throw SheaftException.Validation("Le montant minimum de commande pour appliquer le forfait de livraison doit être supérieur au montant minimum d'acceptation de commande.");

            ApplyDeliveryFeesWhen = feesApplication;
            DeliveryFeesMinPurchaseOrdersAmount = minAmount;
            
            DeliveryFeesWholeSalePrice = fees.HasValue ? Math.Round(fees.Value, 2, MidpointRounding.AwayFromZero) : null;
            DeliveryFeesVatPrice = DeliveryFeesWholeSalePrice.HasValue ? Math.Round(DeliveryFeesWholeSalePrice.Value * 0.20m, 2, MidpointRounding.AwayFromZero) : null;
            DeliveryFeesOnSalePrice = DeliveryFeesWholeSalePrice.HasValue && DeliveryFeesVatPrice.HasValue ? Math.Round(DeliveryFeesWholeSalePrice.Value * 1.20m, 2, MidpointRounding.AwayFromZero) : null;
        }

        public void SetLockOrderHoursBeforeDelivery(int? lockOrderHoursBeforeDelivery)
        {
            LockOrderHoursBeforeDelivery = lockOrderHoursBeforeDelivery;
        }

        public void SetAddress(string line1, string line2, string zipcode, string city, CountryIsoCode country,
            double? longitude = null, double? latitude = null)
        {
            Address = new DeliveryAddress(line1, line2, zipcode, city, country, longitude, latitude);
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetAvailability(bool available)
        {
            Available = available;
        }

        public void SetAutoAcceptRelatedPurchaseOrders(bool autoAccept)
        {
            AutoAcceptRelatedPurchaseOrder = autoAccept;
        }

        public void SetAutoCompleteRelatedPurchaseOrders(bool autoComplete)
        {
            AutoCompleteRelatedPurchaseOrder = autoComplete;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw SheaftException.Validation("Le nom du mode de livraison est requis.");

            Name = name;
        }

        public void SetKind(DeliveryKind kind)
        {
            Kind = kind;
        }

        public void SetMaxPurchaseOrdersPerTimeSlot(int? maxPurchaseOrdersPerTimeSlot)
        {
            MaxPurchaseOrdersPerTimeSlot = maxPurchaseOrdersPerTimeSlot;
        }

        public void AddClosing(DeliveryClosing closing)
        {
            if (Closings == null)
                Closings = new List<DeliveryClosing>();

            Closings.Add(closing);
            ClosingsCount = Closings?.Count ?? 0;
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
                throw SheaftException.NotFound("La plage de fermeture pour ce mode de livraison est introuvable.");

            Closings.Remove(closing);
            ClosingsCount = Closings?.Count ?? 0;
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        public byte[] RowVersion { get; private set; }
    }
}