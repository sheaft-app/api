﻿using Sheaft.Interop;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
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
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Identifier { get; private set; }
        public string Name { get; private set; }
        public WalletKind Kind { get; private set; }
        public decimal? Amount { get; private set; }
        public virtual User User { get; private set; }

        public void SetName(string name)
        {
            if (name == null)
                return;

            Name = name;
        }

        public void SetAmount(decimal amount)
        {
            Amount = amount;
        }

        public void SetIdentifier(string identifier)
        {
            if (identifier == null)
                return;

            Identifier = identifier;
        }
    }
}