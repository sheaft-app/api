using System;
using System.Collections.Generic;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class PreAuthorization : IEntity, IHasDomainEvent
    {
        protected PreAuthorization()
        {
        }

        public PreAuthorization(Guid id, Order order, Card card, string secureModeReturnUrl)
        {
            Id = id;
            Order = order;
            OrderId = order.Id;
            Debited = order.TotalPrice;
            Card = card;
            CardId = card.Id;
            SecureModeReturnURL = secureModeReturnUrl;
            Reference = $"SHFT{DateTime.UtcNow.ToString("DDMMYY")}";
        }

        public Guid Id { get; private set; }
        public string Identifier { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public DateTimeOffset? ExpirationDate { get; private set; }
        public decimal Debited { get; private set; }
        public decimal Remaining { get; private set; }
        public PreAuthorizationStatus Status { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }
        public string Reference { get; private set; }
        public string ResultCode { get; private set; }
        public string ResultMessage { get; private set; }
        public bool SecureModeNeeded { get; private set; }
        public string SecureModeRedirectUrl { get; private set; }
        public string SecureModeReturnURL { get; private set; }
        public bool Processed { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid CardId { get; private set; }
        public Guid? PreAuthorizedPayinId { get; private set; }
        public virtual Order Order { get; private set; }
        public virtual Card Card { get; private set; }
        public virtual PreAuthorizedPayin PreAuthorizedPayin { get; private set; }

        public void SetPreAuthorizedPayin(PreAuthorizedPayin payin)
        {
            if (PreAuthorizedPayinId.HasValue && PreAuthorizedPayin.Status == TransactionStatus.Succeeded)
                throw SheaftException.Validation("Ce prépaiement possède déjà un virement validé.");
            
            if (PreAuthorizedPayinId.HasValue && PreAuthorizedPayin.Status == TransactionStatus.Created)
                throw SheaftException.Validation("Ce prépaiement possède déjà un virement en cours.");

            PreAuthorizedPayin = payin;
            PreAuthorizedPayinId = payin.Id;
        }

        public void SetIdentifier(string identifier)
        {
            if (identifier == null)
                return;

            Identifier = identifier;
            SecureModeReturnURL += $"?preAuthorizationId={Identifier}";
        }

        public void SetStatus(PreAuthorizationStatus status)
        {
            Status = status;
        }

        public void SetPaymentStatus(PaymentStatus paymentStatus)
        {
            PaymentStatus = paymentStatus;
        }

        public void SetSecureModeNeeded(bool needed)
        {
            SecureModeNeeded = needed;
        }

        public void SetExpirationDate(DateTimeOffset? expirationDate)
        {
            if (!expirationDate.HasValue)
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
            if (resultCode == null && resultMessage == null)
                return;

            ResultCode = resultCode;
            ResultMessage = resultMessage;
        }

        public void SetSecureModeRedirectUrl(string secureModeRedirectUrl)
        {
            if (secureModeRedirectUrl == null)
                return;

            SecureModeRedirectUrl = secureModeRedirectUrl;
        }

        public void SetAsProcessed()
        {
            if (Processed)
                throw SheaftException.Conflict("Ce prépaiement a déjà été traité.");

            Processed = true;
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        public byte[] RowVersion { get; private set; }
    }
}