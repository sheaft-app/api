using System;
using Sheaft.Interop;
using Sheaft.Interop.Enums;

namespace Sheaft.Domain.Models
{
    public abstract class Transaction : IEntity
    {
        protected Transaction()
        {
        }

        protected Transaction(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Identifier { get; private set; }
        public decimal Debited { get; set; }
        public decimal Credited { get; set; }
        public decimal Fees { get; set; }
        public string Reference { get; set; }
        public TransactionNature Nature { get; set; }
        public TransactionKind Kind { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTimeOffset? ExecutedOn { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public virtual Wallet CreditedWallet { get; set; }
        public virtual Wallet DebitedWallet { get; set; }
        public virtual User Author { get; set; }

        public void Remove()
        {
        }

        public void Restore()
        {
            RemovedOn = null;
        }
    }
}