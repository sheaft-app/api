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

        protected Transaction(Guid id, TransactionKind kind, TransactionNature nature, User author)
        {
            Id = id;
            Kind = kind;
            Nature = nature;
            Author = author;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Identifier { get; private set; }
        public TransactionNature Nature { get; private set; }
        public TransactionKind Kind { get; private set; }
        public TransactionStatus Status { get; private set; }
        public DateTimeOffset? ExecutedOn { get; private set; }
        public string ResultCode { get; private set; }
        public string ResultMessage { get; private set; }
        public decimal Fees { get; protected set; }
        public string Reference { get; protected set; }
        public decimal Debited { get; protected set; }
        public decimal Credited { get; protected set; }
        public virtual Wallet CreditedWallet { get; protected set; }
        public virtual Wallet DebitedWallet { get; protected set; }
        public virtual User Author { get; private set; }

        public void SetIdentifier(string identifier)
        {
            if (identifier == null)
                return;

            Identifier = identifier;
        }

        public void SetResult(string code, string message)
        {
            ResultCode = code;
            ResultMessage = message;
        }
    }
}