using Sheaft.Domain.Enums;
using Sheaft.Domain.Interop;
using Sheaft.Exceptions;
using System;
using System.Collections.Generic;

namespace Sheaft.Domain.Models
{
    public class PreAuthorization : IEntity
    {
        private List<PreAuthorizedPurchaseOrderPayin> _purchaseOrdersPayins;
        protected PreAuthorization() { }
        
        public PreAuthorization(Guid id, Order order, Card card, string secureModeReturnUrl)
        {
            Id= id;
            Order = order;
            Debited = order.TotalPrice;
            Card = card;
            SecureModeReturnURL = secureModeReturnUrl;
            Reference = $"SHFT{DateTime.UtcNow.ToString("DDMMYY")}";

            _purchaseOrdersPayins = new List<PreAuthorizedPurchaseOrderPayin>();
        }

        public Guid Id { get; private set;}
        public string Identifier { get; private set;}
        public DateTimeOffset CreatedOn { get; private set;}
        public DateTimeOffset? UpdatedOn { get; private set;}
        public DateTimeOffset? RemovedOn { get; private set;}
        public DateTimeOffset? ExpirationDate { get; private set;}
        public decimal Debited { get; private set;}
        public decimal Remaining { get; private set;}
        public PreAuthorizationStatus Status { get; private set;}
        public PaymentStatus PaymentStatus { get; private set;}
        public string Reference { get; private set;}
        public string ResultCode { get; private set;}
        public string ResultMessage { get; private set;}
        public bool SecureModeNeeded { get; private set;}
        public string SecureModeRedirectUrl { get; private set;}
        public string SecureModeReturnURL { get; private set;}
        public virtual Order Order { get; private set;}
        public virtual Card Card { get; private set;}
        public virtual PreAuthorizedDonationPayin DonationPayin  { get; private set;}
        public virtual IReadOnlyCollection<PreAuthorizedPurchaseOrderPayin> PurchaseOrdersPayins => _purchaseOrdersPayins?.AsReadOnly();

        public void SetIdentifier(string identifier)
        {
            if(identifier == null)
                return;

            Identifier = identifier;
        }

        public void SetStatus(PreAuthorizationStatus status)
        {
            Status = status;
        }

        public void SetPaymentStatus(PaymentStatus paymentStatus)
        {
            PaymentStatus = paymentStatus;
        }

        public void SetExpirationDate(DateTimeOffset? expirationDate)
        {
            if(!expirationDate.HasValue)
                return;

            ExpirationDate = expirationDate;
        }

        public void SetRemaining(decimal remaining)
        {
            Remaining = remaining;
        }

        public void SetDebited(decimal debited)
        {
            Debited = debited;
        }

        public void SetResult(string resultCode, string resultMessage)
        {
            if(resultCode == null && resultMessage == null)
                return;

            ResultCode = resultCode;
            ResultMessage = resultMessage;
        }

        public void SetSecureModeRedirectUrl(string secureModeRedirectUrl)
        {
            if(secureModeRedirectUrl == null)
                return;

            SecureModeRedirectUrl = secureModeRedirectUrl;
        }
    }
}