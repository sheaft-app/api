using System;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Wallet : IEntity
    {
        protected Wallet()
        {
        }

        public Wallet(Guid id, string name, WalletKind kind, User user)
        {
            Id = id;
            Name = name;
            Kind = kind;
            User = user;
            UserId = user.Id;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public DateTimeOffset? ExternalUpdatedOn { get; private set; }
        public string Identifier { get; private set; }
        public string Name { get; private set; }
        public WalletKind Kind { get; private set; }
        public decimal Balance { get; private set; }
        public Guid UserId { get; private set; }
        public virtual User User { get; private set; }
        public byte[] RowVersion { get; private set; }

        public void SetName(string name)
        {
            if (name == null)
                return;

            Name = name;
        }

        public void SetAmount(decimal balance)
        {
            Balance = balance;
        }

        public void SetIdentifier(string identifier)
        {
            if (identifier == null)
                return;

            Identifier = identifier;
        }
    }
}