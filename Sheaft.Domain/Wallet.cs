using Sheaft.Interop;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{

    public class Wallet : IEntity
    {
        public Wallet(Guid id, string identifier, string name, WalletKind kind)
        {
            Id = id;
            Identifier = identifier;
            Name = name;
            Kind = kind;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Identifier { get; private set; }
        public string Name { get; private set; }
        public WalletKind Kind { get; private set; }

        public void Remove()
        {

        }

        public void Restore()
        {
            RemovedOn = null;
        }
    }
}