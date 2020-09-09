﻿using Sheaft.Interop;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public abstract class PaymentMethod : IEntity
    {
        protected PaymentMethod()
        {

        }

        protected PaymentMethod(Guid id, string identifier, string name, string owner, PaymentKind kind)
        {
            Id = id;
            Identifier = identifier;
            Name = name;
            Owner = owner;
            Kind = kind;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Identifier { get; private set; }
        public string Name { get; private set; }
        public PaymentKind Kind { get; private set; }
        public string Owner { get; private set; }

        public void Remove()
        {

        }

        public void Restore()
        {
            RemovedOn = null;
        }
    }
}